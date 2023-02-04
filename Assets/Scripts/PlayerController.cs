using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")] [SerializeField]
    private float walkSpeed = 8f;

    [SerializeField] private float runSpeed = 14f;

    [Header("Mouse Control Options")] [SerializeField]
    private float mouseSensivity = 1f;

    [SerializeField] private float maxViewAngle = 60f;
    private CharacterController _characterController;
    private float _currentSpeed = 8f;

    private float _horizontalInput;
    private float _verticalInput;

    private Transform _mainCamera;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        if (Camera.main.GetComponent<CameraController>() == null)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }

        _mainCamera = GameObject.FindWithTag("CameraPoint").transform;
    }

    private void Update()
    {
        KeyboardInput();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    public void Rotate()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + MouseInput().x,
            transform.eulerAngles.z);
        if (_mainCamera != null)
        {

            if (_mainCamera.eulerAngles.x > maxViewAngle && _mainCamera.eulerAngles.x < 180f)
            {
                _mainCamera.rotation =
                    Quaternion.Euler(maxViewAngle, _mainCamera.eulerAngles.y, _mainCamera.eulerAngles.z);
            }
            else if (_mainCamera.eulerAngles.x > 180 && _mainCamera.eulerAngles.x < 360 - maxViewAngle)
            {
                _mainCamera.rotation = Quaternion.Euler(360 - maxViewAngle, _mainCamera.eulerAngles.y,
                    _mainCamera.eulerAngles.z);
            }
            else
            {
                _mainCamera.rotation =
                    Quaternion.Euler(_mainCamera.rotation.eulerAngles + new Vector3(-MouseInput().y, 0f, 0f));
            }
        }
    }

    private Vector2 MouseInput()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensivity;
    }

    private void Move()
    {
        Vector3 localVerticalVector = transform.forward * _verticalInput;
        Vector3 localHorizontalVector = transform.right * _horizontalInput;

        var movementVector = localVerticalVector + localHorizontalVector;
        movementVector.Normalize();
        movementVector *= _currentSpeed * Time.deltaTime;
        _characterController.Move(movementVector);
    }

    private void KeyboardInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        _currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
    }
}