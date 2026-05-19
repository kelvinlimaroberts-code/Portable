using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float jumpSpeed = 2f;
    private bool _jumpTriggered = false;
    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidbody = GetComponent<Rigidbody>();

        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;  //subscribing to the onjumpbuttonpressed event in the input controller script
    }

    private void FixedUpdate()  //inculdes delta time automatically, this is where all physiscs should be done
    {
        Vector3 velocity = new Vector3(_playerInputController.MovementInputVector.x, 0, _playerInputController.MovementInputVector.y)  * speed; // get the desired direction
        velocity.y = _rigidbody.linearVelocity.y; //include gravity

        if(_jumpTriggered)
        {
            velocity.y = jumpSpeed;
            _jumpTriggered = false;
        }


        _rigidbody.linearVelocity = velocity; //applying velocity
    }

    private void JumpButtonPressed()
    {
        if(_groundController.IsGrounded)
        {
            _jumpTriggered = true;
        }
    }
}
