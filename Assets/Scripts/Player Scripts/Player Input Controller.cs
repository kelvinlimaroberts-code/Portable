using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector { get; private set; }
    public event Action OnJumpButtonPressed;
    public Vector2 LookInputVector { get; private set; }
    public event Action OnCrouchButtonPressed;
    public Vector3 mouseDelta { get; private set; }
    public bool isKeyboardAndMouse { get; private set; } = true;
    public event Action OnInteractButtonPressed;
    public event Action OnYellowPortalShot;
    public event Action OnPurplePortalShot;


    private void OnMove(InputValue inputValue)
    {
        MovementInputVector = inputValue.Get<Vector2>();
    }
    private void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnJumpButtonPressed?.Invoke();
        }
    }
    private void OnLook(InputValue inputValue)
    {
        LookInputVector = inputValue.Get<Vector2>();
    }

    private void OnCrouch(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnCrouchButtonPressed?.Invoke();
        }
    }
    
    private void OnInteract(InputValue inputValue)
    {
        if(inputValue.isPressed)
        {
            OnInteractButtonPressed?.Invoke();
        }
    }
 
    private void OnYellowPortal(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnYellowPortalShot?.Invoke();
        }
    }

    private void OnPurplePortal(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnPurplePortalShot?.Invoke();
        }
    }
}
