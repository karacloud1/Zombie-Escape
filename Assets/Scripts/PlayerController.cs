using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")] [SerializeField]
    private float walkSpeed = 8f;

    [SerializeField] private float runSpeed = 14f;
    [SerializeField] private float gravityModifer = 0.95f;
    [SerializeField] private float jumpPower = 0.25f;
    [SerializeField] private InputAction newMovementInput;

    [Header("Mouse Control Options")] [SerializeField]
    private float mouseSensivity = 1f;

    [SerializeField] private float maxViewAngle = 60f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    private CharacterController _characterController;
    private float _currentSpeed = 8f;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _heightMovement;
    private bool _jump = false;

    private Transform _mainCamera;

    private void OnEnable()
    {
        newMovementInput.Enable();
    }

    private void OnDisable()
    {
        newMovementInput.Disable();
    }

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
        return new Vector2(invertX ? -Mouse.current.delta.x.ReadValue() : Mouse.current.delta.x.ReadValue(),
            invertY ? -Mouse.current.delta.y.ReadValue() : Mouse.current.delta.y.ReadValue()) * mouseSensivity;
    }

    private void Move()
    {
        if (_jump)
        {
            _heightMovement.y = jumpPower;
            _jump = false;
        }

        _heightMovement.y -= gravityModifer * Time.deltaTime;
        Vector3 localVerticalVector = transform.forward * _verticalInput;
        Vector3 localHorizontalVector = transform.right * _horizontalInput;

        var movementVector = localVerticalVector + localHorizontalVector;
        movementVector.Normalize();
        movementVector *= _currentSpeed * Time.deltaTime;
        _characterController.Move(movementVector + _heightMovement);
        if (_characterController.isGrounded)
        {
            _heightMovement.y = 0f;
        }
    }

    private void KeyboardInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && _characterController.isGrounded)
        {
            _jump = true;
        }

        _horizontalInput = newMovementInput.ReadValue<Vector2>().x;
        _verticalInput = newMovementInput.ReadValue<Vector2>().y;

        _currentSpeed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : walkSpeed;
    }
}