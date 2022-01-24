using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryWeaponController : MonoBehaviour
{
    [SerializeField]
    private float _rangeEMP;
    private Vector3 _rangeEMPVector;
    private Vector3 _startScale = new Vector3(1,1,1);
    private float _scaleTracker;
    [SerializeField]
    private float _coolDown;
    private float _EMPTimeLastUsed;
    [SerializeField]
    private LayerMask _enemyLayerMask;
    [SerializeField]
    private PoolableObject _empPrefab;

    private void Awake()
    {
        SetupEMP();
    }

    private void OnEnable()
    {
        _scaleTracker = 0f;       
    }

    private void SetupEMP()
    {
        PoolSystem.CreatePool(_empPrefab, 3);
        _rangeEMPVector = new Vector3(1 * _rangeEMP, 1 * _rangeEMP, 1 * _rangeEMP);
        _EMPTimeLastUsed = float.MinValue;
    }

    [ContextMenu("EMP Stun")]
    public void Firing()
    {
        if (CheckEMPAvailability())
        {
            CreateEMPWave();
            var collidersInRange = Physics.OverlapSphere(transform.position, _rangeEMP, _enemyLayerMask);
            foreach (Collider collider in collidersInRange)
            {
                EnemyController enemy = collider.GetComponent<EnemyController>();
                if (enemy != null && enemy.transform.gameObject.activeSelf)
                {
                    enemy.EmpStun();
                }
            }
            _EMPTimeLastUsed = Time.time;
        }
    }

    private bool CheckEMPAvailability()
    {
        float currentTime= Time.time;
        if (currentTime >= _EMPTimeLastUsed + _coolDown)
            return true;
        else
        {
            Debug.Log($" EMP Not available - Cool Down Remaining : {_EMPTimeLastUsed + _coolDown - currentTime}");
            return false;
        }
    }

    public void CreateEMPWave()
    {
        PoolableObject empPrefab = PoolSystem.GetNext(_empPrefab);
        empPrefab.transform.position = Vector3.zero - new Vector3( );
        empPrefab.transform.localScale = _startScale;
        empPrefab.transform.gameObject.SetActive(true);
        StartCoroutine(EMPExplosion(empPrefab));       
    }

    private IEnumerator EMPExplosion(PoolableObject EMP)
    {
        while(EMP.transform.localScale.x < _rangeEMPVector.x)
        {
            Vector3 newScale = Vector3.Lerp(_startScale, _rangeEMPVector, _scaleTracker);
            EMP.transform.localScale = newScale;            
            _scaleTracker += 0.01f;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        EMP.transform.gameObject.SetActive(false);
        yield break;
    }
}
