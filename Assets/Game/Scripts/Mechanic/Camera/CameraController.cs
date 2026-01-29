using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Position")]
    [SerializeField] private float height = 10f;
    [SerializeField] private float tiltAngle = 25f;

    [Header("Distance")]
    [SerializeField] private float minDistance = 10f;
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private float slowReturnSpeed = 2f;
    [SerializeField] private float fastReturnSpeed = 6f;

    private float currentDistance;

    [Header("Rotation")]
    [SerializeField] private float baseRotateSpeed = 60f;

    [Header("Lag")]
    [SerializeField] private float positionLag = 0.25f;
    [SerializeField] private float maxLagDistance = 4f;

    private Vector3 velocity;
    private Vector3 smoothFollowPosition;

    private void Start()
    {
        currentDistance = minDistance;
        smoothFollowPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (!target) return;

        UpdateRotation();
        UpdateDistance();
        UpdatePosition();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        smoothFollowPosition = transform.position;
        velocity = Vector3.zero;
    }

    // ---------------- ROTATION ----------------

    private void UpdateRotation()
    {
        float carYaw = target.eulerAngles.y;
        float cameraYaw = transform.eulerAngles.y;

        float delta = Mathf.DeltaAngle(cameraYaw, carYaw);
        float absDelta = Mathf.Abs(delta);

        float rotateSpeed = baseRotateSpeed;

        if (absDelta > 40f && absDelta < 80f)
        {
            rotateSpeed *= 3f;
        }

        float step = rotateSpeed * Time.deltaTime;
        float newYaw = Mathf.MoveTowardsAngle(cameraYaw, carYaw, step);

        transform.rotation = Quaternion.Euler(tiltAngle, newYaw, 0f);
    }

    // ---------------- DISTANCE ----------------

    private void UpdateDistance()
    {
        float targetDistance = minDistance;
        float speed = slowReturnSpeed;

        if (currentDistance > 20f)
        {
            speed = fastReturnSpeed;
        }

        currentDistance = Mathf.MoveTowards(
            currentDistance,
            targetDistance,
            speed * Time.deltaTime
        );

        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
    }

    // ---------------- POSITION ----------------

    private void UpdatePosition()
    {
        // стабільний forward без pitch/roll
        Vector3 flatForward = Vector3.ProjectOnPlane(
            target.forward,
            Vector3.up
        ).normalized;

        Vector3 desiredPosition =
            target.position
            - flatForward * currentDistance
            + Vector3.up * height;

        Vector3 delta = desiredPosition - smoothFollowPosition;

        if (delta.magnitude > maxLagDistance)
        {
            desiredPosition = smoothFollowPosition + delta.normalized * maxLagDistance;
        }

        smoothFollowPosition = Vector3.SmoothDamp(
            smoothFollowPosition,
            desiredPosition,
            ref velocity,
            positionLag
        );

        // фіксуємо Y
        smoothFollowPosition.y = height;

        transform.position = smoothFollowPosition;
    }
}
