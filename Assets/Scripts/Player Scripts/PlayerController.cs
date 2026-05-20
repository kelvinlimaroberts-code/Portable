using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //movement and jumping
    public float speed = 3f;
    public float jumpSpeed = 2f;
    private bool _jumpTriggered = false;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidbody;

    //camera
    public Transform Head;
    public float mouseSens = 50f;
    private float xRot = 0;
    private float yRot = 0;

    private void Awake()
    {
        //moving + jump
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;  //subscribing to the onjumpbuttonpressed event in the input controller script
         //cam
        Cursor.lockState = CursorLockMode.Locked;  //lock cursor to center of screen
    }




    private void FixedUpdate()  //inculdes delta time automatically, this is where all physiscs should be done
    {
        //inputs 
        Vector3 deltaCamMovement = new Vector3(-_playerInputController.LookInputVector.y, _playerInputController.LookInputVector.x, 0) / 100 * mouseSens;  //camera input
        Vector3 localMovementInput = new Vector3(_playerInputController.MovementInputVector.x, 0, _playerInputController.MovementInputVector.y); //movement input          // get the desired direction 

        //camera turning
        xRot += deltaCamMovement.x; //keep track of up/down looking
        yRot += deltaCamMovement.y; //keep track of left and right looking
        clamp(ref xRot, -90, 90);
        Head.rotation = Quaternion.Euler(xRot, 0, 0);  //turn head up and down
        this.transform.Rotate(0, yRot, 0); //turn body left and right

        //cam dir
        Vector3 forward = Head.forward;
        Vector3 right = Head.right;
        forward.y = 0;
        right.y = 0;
        //calculate movements relative to direction
        Vector3 forwardRelative = localMovementInput.z * forward;
        Vector3 rightRelative = localMovementInput.x * right;
        Vector3 moveDir = forwardRelative + rightRelative;

        //moving
        Vector3 linearVelocity = moveDir * speed;   //put movement relative to direction and speed
        
        linearVelocity.y = _rigidbody.linearVelocity.y; //include gravity
        if(_jumpTriggered)
        {
            linearVelocity.y = jumpSpeed;
            _jumpTriggered = false;
        }
        _rigidbody.linearVelocity = linearVelocity; //applying linear velocity
    }

    private void JumpButtonPressed()
    {
        if(_groundController.IsGrounded)
        {
            _jumpTriggered = true;
        }
    }

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
}
