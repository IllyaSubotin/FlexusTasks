using UnityEngine;

public class WheelView : MonoBehaviour
{
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private float wheelRadius = 0.35f;
    [SerializeField] private LayerMask groundMask;

    private float rotationAngle;

    public void UpdateWheel(Vector3 worldVelocity, float steerAngle, float suspensionLength)
    {
        UpdateRotation(worldVelocity, steerAngle);
    }
    
    private void UpdateRotation(Vector3 velocity, float steerAngle)
    {
        float speed = velocity.magnitude;
        rotationAngle += speed * 360f * Time.deltaTime;

        Quaternion roll = Quaternion.Euler(rotationAngle, 0f, 0f);
        Quaternion steer = Quaternion.Euler(0f, steerAngle, 0f);

        wheelTransform.localRotation = steer * roll;
    }
}
