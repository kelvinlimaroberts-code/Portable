using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector2 MovementInputVector {  get; private set; }
    public event Action OnJumpButtonPressed;
    public Vector2 LookInputVector { get; private set; }

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

}
