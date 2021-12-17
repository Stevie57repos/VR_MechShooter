using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PlayerInput))]
public class PlayerWeaponController : MonoBehaviour
{ 
    private PlayerInput _playerInput;
    private InputAction _primaryTriggerAction;
    private InputAction _toggleSecondaryAction;
    private float _round2decimals = 100f;
    [SerializeField]
    ActionBasedController _rightController;

    [SerializeField]
    private PrimaryGunController _primaryGunRight;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _primaryTriggerAction = _playerInput.actions["FirePrimary"];
        _toggleSecondaryAction = _playerInput.actions["ToggleSecondary"];
    }

    private void OnEnable()
    {
        _primaryTriggerAction.performed += PrimaryWeaponFireRightHand;
        _primaryTriggerAction.canceled += PrimaryWeaponRightStop;
    }

    private void OnDisable()
    {
        _primaryTriggerAction.performed -= PrimaryWeaponFireRightHand;
        _primaryTriggerAction.canceled -= PrimaryWeaponRightStop;
    }

    private void PrimaryWeaponFireRightHand(InputAction.CallbackContext context)
    {
        bool value = context.ReadValueAsButton();
        if (value)
            _primaryGunRight.Firing(_rightController);
    }

    private void PrimaryWeaponRightStop(InputAction.CallbackContext context)
    {
        _primaryGunRight.StopFiring();
    }

    private void ToggleSecondary(InputAction.CallbackContext context)
    {
        string display = "toggle was pressed";
        Debug.Log(display);
        DebugEditorScreen.Instance.DisplayValue($"pew pew. Primary trigger value is {display}");
    }
}
