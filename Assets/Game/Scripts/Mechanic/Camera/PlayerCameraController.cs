using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] public Transform target; // персонаж

    [Header("Camera Settings")]
    [SerializeField] private float height = 10f;          // висота камери
    [SerializeField] private float distance = 6f;         // відстань від персонажа
    [SerializeField] private float rotationSpeed = 5f;    // швидкість обертання мишею

    private float currentYaw = 0f;

    private void LateUpdate()
    {
        if (!target) return;

        HandleRotation();
        UpdatePosition();
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X"); // рух миші по X
        currentYaw += mouseX * rotationSpeed;
    }

    private void UpdatePosition()
    {
        // Позиція камери: за персонажем на відстані distance і висоті height
        Vector3 offset = new Vector3(0f, height, -distance);

        // обертаємо offset навколо Y
        Quaternion rotation = Quaternion.Euler(0f, currentYaw, 0f);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;

        // дивимося на персонажа
        transform.LookAt(target.position + Vector3.up * (height / 2f));
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        transform.parent = newTarget;
    }
}
