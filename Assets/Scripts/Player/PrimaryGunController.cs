using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PrimaryGunController : MonoBehaviour
{
    private float _gunRange;
    private float _damage;

    [SerializeField]
    private float coolDownTime;
    private float nextFireTime = float.MinValue;

    [Header("Muzzle FX")]
    [SerializeField]
    private ParticleSystem _muzzleFlashPS;
    [SerializeField]
    private AudioSource _muzzleSound;
    [SerializeField]
    private AudioClip _gunShot;
    [SerializeField]
    private ParticleSystem _hitImpactPS;
    [SerializeField]
    Vector3 _projectileOffset;

    [SerializeField]
    Transform target;
    [SerializeField]
    Vector3 targetDirection;

    private bool isShooting = false;
    
    [SerializeField]
    private LayerMask _enemyLayer;

    public void Initialize(float gunRange, float damage)
    {
        _gunRange = gunRange;
        _damage = damage;  
    }

    public void Firing(ActionBasedController controller, Vector3 target)
    {
        if(isShooting == false)
        {
            isShooting = true;
            StartCoroutine(Shooting(controller, target));
        }
    }

    private IEnumerator Shooting(ActionBasedController controller, Vector3 target)
    {
        while (isShooting == true)
        {
            if (!CheckGunCoolDown()) yield return null;
            else
            {
                AimAtTarget();
                _muzzleFlashPS.Play();
                _muzzleSound.PlayOneShot(_gunShot);
                controller.SendHapticImpulse(.25f, .25f);
                Ray ray = new Ray(transform.position, transform.forward * _gunRange);
                if (Physics.Raycast(ray, out RaycastHit info, _gunRange))
                {
                    IDamageable enemy = info.transform.GetComponent<IDamageable>();
                    if (enemy != null)
                    {
                        Vector3 knockBack = (info.transform.position - transform.position).normalized * 5f;
                        enemy.TakeDamage(_damage, knockBack);
                    }
                }
                else
                {
                    //DebugEditorScreen.Instance.DisplayValue("Hit Nothing");
                }
            }
        }
    }

    private void AimAtTarget()
    {
        targetDirection = target.position - new Vector3(transform.position.x + _projectileOffset.x, transform.position.y + _projectileOffset.y, transform.position.z);
        _muzzleFlashPS.transform.LookAt(targetDirection);
    }

    public void StopFiring()
    {
        //Debug.Log($"stop firing called");
        StopAllCoroutines();
        isShooting = false;
    }

    private bool CheckGunCoolDown()
    {
        if(Time.time > nextFireTime)
        {
            //Debug.Log($"time is {Time.time} and nextfiretime is : {nextFireTime}");
            nextFireTime = Time.time + coolDownTime;
            return true;
        }
        return false;     
    }
}
