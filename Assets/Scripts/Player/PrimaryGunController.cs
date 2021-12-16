using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryGunController : MonoBehaviour
{
    [SerializeField]
    private float _gunRange;

    [SerializeField]
    private LayerMask _enemyLayer;

    private int _triggeredAmount;

    public void Fire(float value)
    {
        string display = $"Fired {_triggeredAmount}";

        Ray ray = new Ray(transform.position, transform.forward * _gunRange);

        if (Physics.Raycast(ray, out RaycastHit info, _gunRange, _enemyLayer))
        {
            display += $".Hit {info.transform.gameObject.name} ";
            DebugEditorScreen.Instance.DisplayValue($"Hit {display}");
        }
        else
        {
            DebugEditorScreen.Instance.DisplayValue("Hit Nothing");
        }      
    }
}
