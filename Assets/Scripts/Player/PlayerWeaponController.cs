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
    private InputAction _primaryRightTrigger;
    private InputAction _primaryLeftTrigger;
    private InputAction _toggleSecondaryAction;
    [SerializeField]
    private ActionBasedController _rightController;
    [SerializeField]
    private ActionBasedController _leftController;
    [SerializeField]
    private Transform _targetRight;
    [SerializeField]
    private Transform _targetLeft;

    [Header("PrimaryGun Settings")]
    [SerializeField]
    private PrimaryGunController _primaryGunRight;
    [SerializeField]
    private PrimaryGunController _primaryGunLeft;
    [SerializeField]
    private float _gunRange;
    [SerializeField]
    private float _damage;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _primaryRightTrigger = _playerInput.actions["FirePrimaryRight"];
        _primaryLeftTrigger = _playerInput.actions["FirePrimaryLeft"];

        _toggleSecondaryAction = _playerInput.actions["ToggleSecondary"];

        _primaryGunRight.Initialize(_gunRange, _damage);
        _primaryGunLeft.Initialize(_gunRange, _damage);

        _targetRight.position = _primaryGunRight.transform.forward  * _gunRange + new Vector3(0, transform.position.y, 0);
        _targetLeft.position = _primaryGunLeft.transform.forward * _gunRange + new Vector3(0, transform.position.y, 0);
    }

    private void OnEnable()
    {
        _primaryRightTrigger.performed += PrimaryWeaponFireRightHand;
        _primaryRightTrigger.canceled += PrimaryWeaponRightStop;
        _primaryLeftTrigger.performed += PrimaryWeaponFireLeftHand;
        _primaryLeftTrigger.canceled += PrimaryWeaponLeftStop;
    }

    private void OnDisable()
    {
        _primaryRightTrigger.performed -= PrimaryWeaponFireRightHand;
        _primaryRightTrigger.canceled -= PrimaryWeaponRightStop;
        _primaryLeftTrigger.performed -= PrimaryWeaponFireLeftHand;
        _primaryLeftTrigger.canceled -= PrimaryWeaponLeftStop;
    }

    private void PrimaryWeaponFireRightHand(InputAction.CallbackContext context)
    {
        bool value = context.ReadValueAsButton();
        if (value)
            _primaryGunRight.Firing(_rightController, _targetRight.position);
    }

    private void PrimaryWeaponFireLeftHand(InputAction.CallbackContext context)
    {
        bool value = context.ReadValueAsButton();
        Debug.Log($"left hand valeu is {value}");
        if (value)
            _primaryGunLeft.Firing(_leftController, _targetLeft.position);
    }

    private void PrimaryWeaponRightStop(InputAction.CallbackContext context)
    {
        _primaryGunRight.StopFiring();
    }
    private void PrimaryWeaponLeftStop(InputAction.CallbackContext context)
    {
        _primaryGunLeft.StopFiring();
    }

    private void ToggleSecondary(InputAction.CallbackContext context)
    {
        string display = "toggle was pressed";
        Debug.Log(display);
        DebugEditorScreen.Instance.DisplayValue($"pew pew. Primary trigger value is {display}");
    }
}
