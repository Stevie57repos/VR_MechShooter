using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimator : MonoBehaviour
{
    public float speed = 5.0f;
    public InputActionReference gripInputReference = null;
    [SerializeField] private float gripValue = 0f;
    public InputActionReference pointerInputReference = null;
    [SerializeField] private float pointerValue = 0f;

    private Animator animator = null;

    private readonly List<Finger> gripFingers = new List<Finger>()
    {
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky)
    };
    private readonly List<Finger> pointFingers = new List<Finger>()
    {
        new Finger(FingerType.Index),
        new Finger(FingerType.Thumb)
    };
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //gripInputReference.action.started += CheckGrip;
        gripInputReference.action.performed += CheckGrip;
        gripInputReference.action.canceled += CheckGrip;

        //pointerInputReference.action.started += CheckPointer;
        pointerInputReference.action.performed += CheckPointer;
        pointerInputReference.action.canceled += CheckPointer;
    }
    private void OnDisable()
    {
        //gripInputReference.action.started -= CheckGrip;
        gripInputReference.action.performed -= CheckGrip;
        gripInputReference.action.canceled -= CheckGrip;

        //pointerInputReference.action.started -= CheckPointer;
        pointerInputReference.action.performed -= CheckPointer;
        pointerInputReference.action.canceled -= CheckPointer;
    }
    private void CheckGrip(InputAction.CallbackContext context)
    {
        //Debug.Log("checkgrip has beenn called");
        gripValue = context.ReadValue<float>();
        //DisplayValue(gripValue);
        SetFingerTargets(gripFingers, gripValue);
        SmoothFinger(gripFingers);
        AnimateFinger(gripFingers);
    }
    private void DisplayValue(float value) { if (value > 0.01) Debug.Log($"value is {value}"); }
    private void CheckPointer(InputAction.CallbackContext context)
    {
        //Debug.Log("CheckPointer has beenn called");
        pointerValue = context.ReadValue<float>();
        SetFingerTargets(pointFingers, pointerValue);
        SmoothFinger(pointFingers);
        AnimateFinger(pointFingers);
    }
    private void SetFingerTargets(List<Finger> fingers, float value)
    {
        foreach (Finger finger in fingers)
            finger.target = value;
    }
    private void SmoothFinger(List<Finger> fingers)
    {
        foreach(Finger finger in fingers)
        {
            float time = speed * Time.unscaledDeltaTime;
            finger.current = Mathf.MoveTowards(finger.current, finger.target, time);
        }
    }
    private void AnimateFinger(List<Finger> fingers)
    {
        foreach (Finger finger in fingers)
            AnimateFinger(finger.type.ToString(), finger.current);
    }
    private void AnimateFinger(string finger, float blend)
    {
        animator.SetFloat(finger, blend);
    }
}