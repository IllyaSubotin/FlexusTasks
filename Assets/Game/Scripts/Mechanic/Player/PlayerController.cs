using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 4f;
    [SerializeField] private float _sprintMultiplier = 1.6f;
    [SerializeField] private float _rotationSpeed = 720f;

    [Header("Components")]
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator _animator;

    private PlayerCameraController _playerCameraController;
    private Camera _activeCamera;
    private bool _movementBlocked;

    public void SetCamera(Camera playerCamera, PlayerCameraController cameraController)
    {
        _activeCamera = playerCamera;
        _playerCameraController = cameraController;
        _playerCameraController.target = transform;
    }

    public void SetCameraDefault()
    {
        _playerCameraController.target = transform;
    }

    private void Update()
    {
        if (_movementBlocked)
        {
            UpdateAnimator(0f, false);
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(h, 0f, v);
        float inputMagnitude = Mathf.Clamp01(inputDir.magnitude);

        if (inputMagnitude < 0.01f)
        {
            UpdateAnimator(0f, false);
            return;
        }

        Vector3 camForward = Vector3.ProjectOnPlane(_activeCamera.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);

        Vector3 moveDir = (camForward * inputDir.z + camRight * inputDir.x).normalized;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = _walkSpeed * (isSprinting ? _sprintMultiplier : 1f);

        _controller.Move(moveDir * speed * Time.deltaTime);

        RotateTowards(moveDir);
        UpdateAnimator(inputMagnitude, isSprinting);
    }

    private void RotateTowards(Vector3 move)
    {
        Quaternion targetRotation = Quaternion.LookRotation(move);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            _rotationSpeed * Time.deltaTime
        );
    }

    private void UpdateAnimator(float speed, bool sprint)
    {
        _animator.SetFloat("Speed", speed);
        _animator.SetBool("Sprint", sprint);
    }

    public void BlockMovement(bool isBlock)
    {
        _movementBlocked = isBlock;
    }

    


}
