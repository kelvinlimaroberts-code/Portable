using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    private PlayerInputController _playerInputController;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
    }

    private void Update()
    {
        Vector3 _positionChange = new Vector3(_playerInputController.MovementInputVector.x, 0, _playerInputController.MovementInputVector.y) 
            * Time.deltaTime * speed;
        transform.position += _positionChange;
    }
}
