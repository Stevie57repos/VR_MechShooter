using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerFOVCheck : MonoBehaviour
{
    [SerializeField]
    private FieldOfView _fieldOfViewHandler;
    [SerializeField]
    private MeshRenderer _meshRenderer;

    // Update is called once per frame
    void Update()
    {
        CheckPlayerFOV();
    }

    private void CheckPlayerFOV()
    {
        if (_fieldOfViewHandler.CheckPosition(transform.position))
            _meshRenderer.material.color = Color.green;
        else
            _meshRenderer.material.color = Color.red;
    }
}
