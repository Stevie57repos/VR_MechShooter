using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{ 
    private PlayerInput _playerInput;
    private InputAction _primaryRightTrigger;
    private InputAction _primaryLeftTrigger;
    private InputAction _secondaryButtonTrigger;

    [SerializeField]
    private ActionBasedController _rightController;
    [SerializeField]
    private ActionBasedController _leftController;
    [SerializeField]
    private Transform _targetRight;
    [SerializeField]
    private Transform _targetLeft;
    [SerializeField]
    private HealthHandler _healthHandler;

    [Header("PrimaryGun Settings")]
    [SerializeField]
    private PrimaryGunController _primaryGunRight;
    [SerializeField]
    private PrimaryGunController _primaryGunLeft;
    [SerializeField]
    private SecondaryWeaponController _secondaryWeapon;

    [SerializeField]
    private float _gunRange;
    [SerializeField]
    private float _damage;

    [SerializeField]
    private PlayerDeathEvenChannelSO _playerDeath;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _primaryRightTrigger = _playerInput.actions["FirePrimaryRight"];
        _primaryLeftTrigger = _playerInput.actions["FirePrimaryLeft"];
        _secondaryButtonTrigger = _playerInput.actions["ToggleSecondary"];

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

        _secondaryButtonTrigger.performed += SeocondaryWeaponFire;
    }

    private void OnDisable()
    {
        _primaryRightTrigger.performed -= PrimaryWeaponFireRightHand;
        _primaryRightTrigger.canceled -= PrimaryWeaponRightStop;
        
        _primaryLeftTrigger.performed -= PrimaryWeaponFireLeftHand;
        _primaryLeftTrigger.canceled -= PrimaryWeaponLeftStop;

        _secondaryButtonTrigger.performed -= SeocondaryWeaponFire;
    }

    private void PrimaryWeaponFireRightHand(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValueAsButton();
        if (isPressed)
            _primaryGunRight.Firing(_rightController, _targetRight.position);
    }

    private void SeocondaryWeaponFire(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValueAsButton();
        if (isPressed)
        {
            _secondaryWeapon.Firing();
        }
    }

    private void PrimaryWeaponFireLeftHand(InputAction.CallbackContext context)
    {
        bool value = context.ReadValueAsButton();
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

    public bool CheckPlayerHealthStatus()
    {
        return _healthHandler.IsAlive();
    }

    public void PlayerDamage(float damage)
    {
        bool isAlive = _healthHandler.TakeDamage(damage);
        _rightController.SendHapticImpulse(.5f, 1f);
        _leftController.SendHapticImpulse(.5f, 1f);

        if (!isAlive)
        {
            Debug.Log($"player is dead !");
            _playerDeath.RaiseEvent();
        }
    }
}
