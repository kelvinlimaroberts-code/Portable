using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    //movement and jumping 
    [Header("movement and jumping")]
    public float speed = 3f;
    public float jumpYPwr = 6f;
    public float jumpBoostPwr = 1f;
    public float jumpDrag = 0.95f;
    private float jumpBoostVelocity = 0f;
    private bool _jumpTriggered = false;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidbody;
    private CapsuleCollider _capsuleCollider;
    private Vector3 moveDir = Vector3.forward;
    private Vector3 calculatedLinearVelocity;
    //crouching
    [Header("crouching")]
    public float standingHeight = 2f;
    public float crouchingHeight = 1f;
    public Vector3 standingCameraPos;
    public Vector3 crouchingCameraPos;

    //interacting
    [Header("interacting")]
    public Transform _interactionArea; //the transform that the player uses to reach and interact with objects
    [SerializeField] private string holdableObjectTag;  //the tag that interactables will have
    private PickUp? _currentPickup;
    private bool _currentlyLookingAtPickup = false;
    private bool _itemIsBeingHeld = false;

    //camera
    [Header("camera")]
    public Transform Head;
    public float controllerSens = 1.5f;
    public float mouseSens = 1.6f;
    private float xRot = 0;  //final x rotation delta
    private float yRot = 0;  //final y rotation delta
    private Vector2 additionalRotation; //used in case tghe portal script needs to rotate the player
    //anim states
    public event Action StateChange;
    public bool isWalking { get; private set; } = false;
    public bool isCrouching { get; private set; } = false;
    private bool prevWalkingState = false;




    private void clamp(ref float value, float min, float max)  // have to put ref infront of variable to call
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }
    }

    private void Awake()
    {
        //get components needed 
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        //moving
        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;  //subscribing to the onjumpbuttonpressed event in the input controller script
        _playerInputController.OnCrouchButtonPressed += CrouchButtonPressed;  //subscribe to crouch event
        _playerInputController.OnInteractButtonPressed += InteractButtonPressed; //subscribe to interact event
        //cam
        Cursor.lockState = CursorLockMode.Locked;  //lock cursor to center of screen
    }

    private void Update()
    {        
        //inputs 
        Vector3 localMovementInput = new Vector3(_playerInputController.MovementInputVector.x, 0, _playerInputController.MovementInputVector.y); //movement input          // get the desired direction 
        Vector3 deltaCamMovement = new Vector3(-_playerInputController.LookInputVector.y, _playerInputController.LookInputVector.x, 0) * controllerSens;  //camera input
        
        //camera turning
        clamp(ref deltaCamMovement.x, -mouseSens, mouseSens);  //cap mouse delta registration
        clamp(ref deltaCamMovement.y, -mouseSens, mouseSens);
        xRot += deltaCamMovement.x + additionalRotation.x; //keep track of up/down looking
        yRot += deltaCamMovement.y + additionalRotation.y; //keep track of left and right looking
        clamp(ref xRot, -90, 90);
        Head.rotation = Quaternion.Euler(xRot, yRot, 0);  //turn head up and down +left and right
        this.transform.rotation = Quaternion.Euler(0, yRot, 0); //turn body left and right
        additionalRotation = new Vector2(0f, 0f); //reset any additional rotation needed

        //cam dir
        Vector3 forward = this.transform.forward;
        Vector3 right = this.transform.right;
        forward.y = 0;
        right.y = 0;
        //calculate movements relative to direction
        Vector3 forwardRelative = localMovementInput.z * forward;
        Vector3 rightRelative = localMovementInput.x * right;
        moveDir = forwardRelative + rightRelative;

        //anim state update
        if (moveDir.x == 0 && moveDir.z == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
        if (isWalking != prevWalkingState)
        {
            StateChange?.Invoke();
        }
        prevWalkingState = isWalking;
    }


    private void FixedUpdate()  //inculdes delta time automatically, this is where all physiscs should be done
    {
        //moving
        calculatedLinearVelocity = new Vector3(0, 0, 0);
        if (_groundController.IsGrounded) //if on ground, walk as normal
        {
            calculatedLinearVelocity = moveDir * (speed + jumpBoostVelocity);   //put movement relative to direction and speed
        }
        else //otherwise, conserve linear momentum
        {
            calculatedLinearVelocity = _rigidbody.linearVelocity; //conserving momentum in air
        }
        jumpBoostVelocity *= jumpDrag;  //reduce jump speed boost by drag

        calculatedLinearVelocity.y = _rigidbody.linearVelocity.y; //include gravity
        if (_jumpTriggered) //detect jump
        {
            jumpBoostVelocity = jumpBoostPwr;
            calculatedLinearVelocity.y = jumpYPwr;
            _jumpTriggered = false;
        }
        _rigidbody.linearVelocity = calculatedLinearVelocity; //applying calculatedLinearVelocity
    }

    public void EditRotation(float xIncrease, float yIncrease)
    {
        additionalRotation.x = xIncrease;
        additionalRotation.y = yIncrease;
    }

    public void EditLinearMomentum(Vector3 newDir)
    {
        _rigidbody.linearVelocity = newDir * (calculatedLinearVelocity.x + calculatedLinearVelocity.y + calculatedLinearVelocity.z);
    }

    private void JumpButtonPressed()
    {
        if(_groundController.IsGrounded)
        {
            _jumpTriggered = true;
        }
    }
    private void CrouchButtonPressed()
    {
        if (isCrouching)
        {
            isCrouching = false;  //become standing
        }
        else
        {
            isCrouching = true;   //become crouching
        }
        StateChange?.Invoke();
        toggleCrouch();
    }
    private void toggleCrouch()
    {
        if (isCrouching) //crouching
        {
            SetCapsuleDimensions(crouchingHeight);
            speed = speed / 2;
            jumpYPwr = jumpYPwr / 2;
        }
        else  //standing
        {
            SetCapsuleDimensions(standingHeight);
            speed = speed * 2;
            jumpYPwr = jumpYPwr * 2;
        }
        Invoke(nameof(toggleCamHeight), 0.29f);
    }
    private void toggleCamHeight()
    {
        if (isCrouching)
        {
            Head.localPosition = crouchingCameraPos;
        }
        else
        {
            Head.localPosition = standingCameraPos;
        }
    }
    private void SetCapsuleDimensions(float height)
    {
        _capsuleCollider.height = height;
        _capsuleCollider.center = new Vector3(0, height/2, 0);
    }

    private void InteractButtonPressed()
    {
        if(_itemIsBeingHeld)
        {
            //drop item
            _currentPickup.BeingHeld(false, null); //revert rigid body physics
            _itemIsBeingHeld = false;
        }
        else if(_currentlyLookingAtPickup)
        {
            //want to pick up item
            _itemIsBeingHeld = true;
            _currentPickup.BeingHeld(true, _interactionArea);   //turn off certain physics so it acts like it's held
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(holdableObjectTag))
        {
            _currentlyLookingAtPickup = true;
            _currentPickup = other.gameObject.GetComponent<PickUp>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(holdableObjectTag))
        {
            _currentlyLookingAtPickup = false;
        }
    }
}
