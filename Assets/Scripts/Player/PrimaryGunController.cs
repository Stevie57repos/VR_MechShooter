using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryGunController : MonoBehaviour
{
    [Header("PrimaryGun Settings")]
    [SerializeField]
    private float _gunRange;
    [SerializeField]
    private float _fireRate = 1f;
    private float _gunCoolDownTracker = 0f;

    [Header("Muzzle FX")]
    [SerializeField]
    private ParticleSystem _muzzleFlashPS;
    [SerializeField]
    private AudioSource _muzzleSound;

    [SerializeField]
    private LayerMask _enemyLayer;

    private int _triggeredAmount;

    public void Fire(float value)
    {
        if(!CheckGunCoolDown()) return;

        //debugging
        string display = $"Fired {_triggeredAmount}";
        _triggeredAmount++;

        Ray ray = new Ray(transform.position, transform.forward * _gunRange);
        _muzzleFlashPS.Play();
        _muzzleSound.Play();
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

    private bool CheckGunCoolDown()
    {
        bool canShoot = false;

        if (_gunCoolDownTracker <= 0f)
        {
            _gunCoolDownTracker = _fireRate;
            canShoot = true;
            return canShoot;
        }
        _gunCoolDownTracker -= Time.deltaTime;
        return canShoot;
    }
}
