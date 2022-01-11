using UnityEngine;

public class BuffTester : MonoBehaviour
{
    [SerializeField]
    private BaseObject _target;

    [SerializeField]
    private float _damage;

    [SerializeField]
    private float _heal;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) // simulate adding a DOT
        {
            _target.AddBuff(5, 3, () => _target.OnHit(_damage), null);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // simulate adding a DOT
        {
            Debug.Log($"started At {Time.time}");
            _target.AddBuff(3, 1, null, () => Debug.Log($"Time has passed now it is: [{Time.time}]"));
        }
    }
}
