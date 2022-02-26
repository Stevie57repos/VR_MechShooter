using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public bool TakeDamage(float damage);
    public void TakeDamage(float damage, Vector3 knockBack);
}
