using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialPartitioner : MonoBehaviour
{
    private List<FlockController> _flockList;

    // limits for grid
    private Vector3 xMin;
    private Vector3 xMax;
    private Vector3 yMin;
    private Vector3 yMax;
    private Vector3 zMin;
    private Vector3 zMax;
    private Vector3 MarkerCubeSize = new Vector3(.25f, 0.25f, 0.25f);

    [Header("Debugging Draw Grid Settings")]
    [SerializeField]
    private bool _debugDrawGrid;
    [SerializeField]
    private int _gridX;
    [SerializeField]
    private int _gridY;
    [SerializeField]
    private int _gridZ;
    [SerializeField]
    private Vector3 _gridCubeSize;
    [SerializeField]
    private Vector3 _gridStartPosition;
    private List<FlockController>[,,] _cells;
    private Vector2 _debugVector;
    private int _counter;

    public void Initialize(List<FlockController> flockList)
    {
        GenerateCells();
        GridLimitCalculation();
        _flockList = flockList;
    }

    private void GenerateCells()
    {
        _cells = new List<FlockController>[_gridX, _gridY, _gridZ];

        for (int x = 0; x < _gridX; x++)
        {
            for (int y = 0; y < _gridY; y++)
            {
                for (int z = 0; z < _gridZ; z++)
                {
                    List<FlockController> list = new List<FlockController>();
                    _cells[x, y, z] = list;
                }
            }
        }
    }

    void GridLimitCalculation()
    {
        xMin = new Vector3(transform.position.x + _gridStartPosition.x, 0, 0);
        xMax = new Vector3(xMin.x + _gridCubeSize.x * _gridX, 0, 0);
        yMin = new Vector3(xMin.x, transform.position.y + _gridStartPosition.y, 0);
        yMax = new Vector3(xMin.x, yMin.y + _gridCubeSize.y * _gridY, 0);
        zMin = new Vector3(xMin.x, 0, transform.position.z + _gridStartPosition.z);
        zMax = new Vector3(xMin.x, 0, zMin.z + _gridCubeSize.z * _gridZ);
    }

    public void ProcessFlockRequest(FlockRequest request)
    {
        FlockCallbackResult result = new FlockCallbackResult();
        FlockController flock = _flockList[request.UnitID];
        bool isInGridRange = CheckInGridRange(flock);

        if (isInGridRange)
        {
            result.InGridRange = true;
            result.FlockList = GetFlockList(flock);
        }
        else
            result.InGridRange = false;

        result.TargetPosition = request.TargetPosition;

        flock.HandleResult(result);
        
        _counter++;
              
    }

    private List<FlockController> GetFlockList(FlockController flock)
    {
        Vector3 cellPos = GetCellPosition(flock);
        List<FlockController> cellFlockList = RetreiveListFromCellPos(cellPos);
        if (!CheckForListDuplicates(cellFlockList, flock))
        {
            // False = Enter a new cell that it wasn't in before
            cellFlockList.Add(flock);

            if(flock.isInitialized == false)
            {
                flock.isInitialized = true;
                flock.PreviousCellList = cellPos;
                return cellFlockList;
            }
            List<FlockController> previousList = RetreiveListFromCellPos(flock.PreviousCellList);
            previousList.Remove(flock);
            flock.PreviousCellList = cellPos;
        }  
        return cellFlockList;
    }

    private bool CheckInGridRange(FlockController flock)
    {
        Vector3 cellPos = GetCellPosition(flock);
        // if outside of grid range
        if (cellPos.x < 0 || cellPos.x > _gridX -1 ||
            cellPos.y < 0 || cellPos.y > _gridY -1 ||
            cellPos.z < 0 || cellPos.z > _gridZ -1  )
        {
            try
            {   // remove the flock from any cell list it is stored on 
                if (flock.isInitialized == true) 
                { 
                    List<FlockController> previousList = RetreiveListFromCellPos(flock.PreviousCellList);
                    previousList.Remove(flock);
                }
            }
            catch { }         
            return false;
        }
        return true;
    }

    private Vector3 GetCellPosition(FlockController flock)
    {
        Vector3 _cellPos = new Vector3(0, 0, 0);
        // get the position on a range
        Vector3 position = flock.transform.position;
        float xPosition = position.x - xMin.x;
        float yPosition = position.y - yMin.y;
        float zPosition = position.z - zMin.z;

        // divide the position by the tile size and floor the value
        float xCell = Mathf.FloorToInt(xPosition / _gridCubeSize.x);
        float yCell = Mathf.FloorToInt(yPosition / _gridCubeSize.y);
        float zCell = Mathf.FloorToInt(zPosition / _gridCubeSize.z);

        _cellPos.x = xCell;
        _cellPos.y = yCell;
        _cellPos.z = zCell;
        return _cellPos;
    }

    private List<FlockController> RetreiveListFromCellPos(Vector3 cellPos)
    {
        List<FlockController> list;
        try
        {
            list = _cells[(int)cellPos.x, (int)cellPos.y, (int)cellPos.z];
        }
        catch 
        {
            return null;
        }
       
        return list;
    }
    private bool CheckForListDuplicates(List<FlockController> cellFlockList, FlockController flock)
    {
        return cellFlockList.Contains(flock) ;
    }

    private void OnDrawGizmos()
    {
        if (_debugDrawGrid)
        {
            Color gridColor = Color.white;
            gridColor.a = 0.05f;
            Gizmos.color = gridColor;

            _gridCubeSize = _gridCubeSize == Vector3.zero ? new Vector3(1, 1, 1) : _gridCubeSize;

            float xPos = _gridStartPosition.x + _gridCubeSize.x/2;
            for (int x = 0; x < _gridX; x++)
            {
                float yPos = _gridStartPosition.y + _gridCubeSize.y/2;
                for (int y = 0; y < _gridY; y++)
                {
                    float zPos = _gridStartPosition.z + _gridCubeSize.z/2;
                    for (int z = 0; z < _gridZ; z++)
                    {
                        Gizmos.DrawWireCube(new Vector3(xPos, yPos, zPos), _gridCubeSize);
                        zPos += _gridCubeSize.z;                        
                    }
                    yPos += _gridCubeSize.y;
                }
                xPos += _gridCubeSize.x;
            }

            // Mark limits
            Gizmos.color = Color.green;
            Gizmos.DrawCube(xMin, MarkerCubeSize);
            Gizmos.DrawCube(xMax, MarkerCubeSize);
            Gizmos.DrawCube(yMin, MarkerCubeSize);
            Gizmos.DrawCube(yMax, MarkerCubeSize);
            Gizmos.DrawCube(zMin, MarkerCubeSize);
            Gizmos.DrawCube(zMax, MarkerCubeSize);
        }
    }
    private void DebugListCount()
    {
        Vector3 list1 = new Vector3(0, 0, 0);
        Vector3 list2 = new Vector3(1, 0, 0);
        Vector3 list3 = new Vector3(2, 0, 0);
        Vector3 list4 = new Vector3(3, 0, 0);
        string display =
            $"Count list 1 is {RetreiveListFromCellPos(list1).Count}<br>" +
            $"Count list 2 is {RetreiveListFromCellPos(list2).Count}<br>" +
            $"Count list 3 is {RetreiveListFromCellPos(list3).Count}<br>" +
            $"Count list 4 is {RetreiveListFromCellPos(list4).Count}<br>";
    }
}

