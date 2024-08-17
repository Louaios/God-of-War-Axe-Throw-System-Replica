using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public event Action throwingEvent;

    private Controls controls;

    [field: SerializeField] public bool isAiming {  get; private set; }

    public Vector2 movementValue {  get; private set; }

    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);

        controls.Enable();
    }

    private void OnDestroy()
    {
        controls.Disable();
    }
    public void OnMovement(InputAction.CallbackContext context)
    {
        movementValue = context.ReadValue<Vector2>();
    }


    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAiming = true;
        } 
        else if (context.canceled)
        {
            isAiming= false;
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if(!context.performed) { return; }

        throwingEvent?.Invoke();
    }
}
