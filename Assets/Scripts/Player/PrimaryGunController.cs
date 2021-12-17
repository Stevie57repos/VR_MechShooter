using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PrimaryGunController : MonoBehaviour
{
    [Header("PrimaryGun Settings")]
    [SerializeField]
    private float _gunRange;
    [SerializeField]
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

    private bool isShooting = false;
    
    [SerializeField]
    private LayerMask _enemyLayer;

    public void Firing(ActionBasedController controller)
    {
        if(isShooting == false)
        {
            isShooting = true;
            StartCoroutine(Shooting(controller));
        }
    }

    private IEnumerator Shooting(ActionBasedController controller)
    {
        while (isShooting == true)
        {
            if (!CheckGunCoolDown()) yield return null;
            else
            {
                _muzzleFlashPS.Play();
                _muzzleSound.PlayOneShot(_gunShot);
                controller.SendHapticImpulse(.25f, .25f);
                Ray ray = new Ray(transform.position, transform.forward * _gunRange);
                if (Physics.Raycast(ray, out RaycastHit info, _gunRange))
                {
                    EnemyController enemy = info.transform.GetComponent<EnemyController>();
                    if (enemy != null)
                        enemy.TakeDamage(_damage);

                    DebugEditorScreen.Instance.DisplayValue($"Hit {info.transform.gameObject.name}");
                }
                else
                {
                    DebugEditorScreen.Instance.DisplayValue("Hit Nothing");
                }
            }
        }
    }

    public void StopFiring()
    {
        Debug.Log($"stop firing called");
        StopAllCoroutines();
        isShooting = false;
    }

    private bool CheckGunCoolDown()
    {
        if(Time.time > nextFireTime)
        {
            Debug.Log($"time is {Time.time} and nextfiretime is : {nextFireTime}");
            nextFireTime = Time.time + coolDownTime;
            return true;
        }
        return false;     
    }

    private void Update()
    {
      
    }
}
