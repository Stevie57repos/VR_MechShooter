using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerWeaponController : MonoBehaviour
{ 
    private PlayerInput _playerInput;
    private InputAction _primaryTriggerAction;
    private InputAction _toggleSecondaryAction;
    private float _round2decimals = 100f;

    [SerializeField]
    private PrimaryGunController _primaryGunRight;

    

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _primaryTriggerAction = _playerInput.actions["FirePrimary"];
        _toggleSecondaryAction = _playerInput.actions["ToggleSecondary"];

        //toggleSecondaryReference.action.started += ToggleSecondary;
        //primaryTriggerReference.action.performed += PrimaryWeaponFire;
    }

    private void OnEnable()
    {
        _primaryTriggerAction.performed += PrimaryWeaponFire;
    }

    private void OnDisable()
    {
        _primaryTriggerAction.performed -= PrimaryWeaponFire;
    }

    private void Update()
    {
       
    }
    private void PrimaryWeaponFire(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        value = Mathf.Round(value * _round2decimals) / _round2decimals;
        if (value > 0.9f)
            _primaryGunRight.Fire(value);
    }

    private void OnDestroy()
    {
        //toggleSecondaryReference.action.started -= ToggleSecondary;
        //primaryTriggerReference.action.performed -= PrimaryWeaponFire;


    }

    private void ToggleSecondary(InputAction.CallbackContext context)
    {
        string display = "toggle was pressed";
        Debug.Log(display);
        DebugEditorScreen.Instance.DisplayValue($"pew pew. Primary trigger value is {display}");
    }
}
