using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    EnemyDeathEventSO _deathEventChannel;
    [SerializeField]
    GameObject _explosion;

    private void OnEnable()
    {
        _deathEventChannel.EnemyDeathEvent += Explode;
    }

    private void OnDisable()
    {
        _deathEventChannel.EnemyDeathEvent -= Explode;
    }

    private void Explode(EnemyController enemy)
    {
        _explosion.transform.position = enemy.transform.position;
        _explosion.SetActive(true);
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(3f);
        _explosion.SetActive(false);
    }



}
