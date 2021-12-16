using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public InputActionReference primaryTriggerReference = null;
    public InputActionReference toggleSecondaryReference = null;
    private float _round2decimals = 100f;

    [SerializeField]
    private PrimaryGunController _primaryGun;

    private void Awake()
    {
        toggleSecondaryReference.action.started += ToggleSecondary;
        primaryTriggerReference.action.performed += PrimaryWeaponFire;
    }

    private void Update()
    {
       
    }
    private void PrimaryWeaponFire(InputAction.CallbackContext context)
    {
        float value = primaryTriggerReference.action.ReadValue<float>();
        value = Mathf.Round(value * _round2decimals) / _round2decimals;
        if (value > 0.9f)
            _primaryGun.Fire(value);
    }

    private void OnDestroy()
    {
        toggleSecondaryReference.action.started -= ToggleSecondary;
        primaryTriggerReference.action.performed -= PrimaryWeaponFire;
    }

    private void ToggleSecondary(InputAction.CallbackContext context)
    {
        string display = "toggle was pressed";
        Debug.Log(display);
        DebugEditorScreen.Instance.DisplayValue($"pew pew. Primary trigger value is {display}");
    }
}
