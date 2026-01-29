using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private float maxSpeed = 20f;

    [Header("Steering")]
    [SerializeField] private float turnSpeed = 120f;

    [Header("Braking")]
    [SerializeField] private float brakeStrength = 10f;

    [Header("Grip")]
    [SerializeField] private float lateralGrip = 6f;

    [Header("Wheels")]
    [SerializeField] private WheelView frontLeft;
    [SerializeField] private WheelView frontRight;
    [SerializeField] private WheelView rearLeft;
    [SerializeField] private WheelView rearRight;

    [SerializeField] private float suspensionLength = 0.5f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Camera")]
    [SerializeField] private PlayerCameraController _camera; 

    [SerializeField] public Rigidbody carRigidbody;
    private bool isBlock = true;

    private void FixedUpdate()
    {
        if(!isBlock)
        {
            float vertical = Input.GetAxis("Vertical");    
            float horizontal = Input.GetAxis("Horizontal");
            bool brake = Input.GetButton("Jump");           

            HandleAcceleration(vertical);
            HandleSteering(horizontal);
            HandleGrip();
            HandleBraking(brake);
            LimitSpeed();

            UpdateWheels(horizontal);
        }
    }

    private void HandleAcceleration(float input)
    {
        if (Mathf.Abs(input) < 0.01f)
            return;

        carRigidbody.AddForceAtPosition(transform.forward * input * acceleration,transform.position - transform.up * 0.5f,ForceMode.Acceleration);
    }

    private void HandleSteering(float input) 
    { 
        float speedFactor = carRigidbody.linearVelocity.magnitude / maxSpeed; 
        
        if (speedFactor < 0.1f) 
            return; 
        
        float turn = input * turnSpeed * speedFactor * Time.fixedDeltaTime; 
        
        carRigidbody.MoveRotation(carRigidbody.rotation * Quaternion.Euler(0f, turn, 0f)); 
    }


    private void HandleGrip()
    {
        Vector3 localVel = transform.InverseTransformDirection(carRigidbody.linearVelocity);

        float grip = lateralGrip;
        localVel.x = Mathf.Lerp(localVel.x, 0, grip * Time.fixedDeltaTime);

        carRigidbody.linearVelocity = transform.TransformDirection(localVel);
    }


    private void HandleBraking(bool brake)
    {
        if (!brake)
            return;

        carRigidbody.linearVelocity = Vector3.Lerp(carRigidbody.linearVelocity, Vector3.zero, brakeStrength * Time.fixedDeltaTime);
    }

    private void LimitSpeed()
    {
        if (carRigidbody.linearVelocity.magnitude > maxSpeed)
        {
            carRigidbody.linearVelocity = carRigidbody.linearVelocity.normalized * maxSpeed;
        }
    }

    private void UpdateWheels(float horizontal)
    {
        float steerAngle = horizontal * maxSteerAngle;

        frontLeft.UpdateWheel(carRigidbody.linearVelocity, steerAngle, suspensionLength);
        frontRight.UpdateWheel(carRigidbody.linearVelocity, steerAngle, suspensionLength);

        rearLeft.UpdateWheel(carRigidbody.linearVelocity, 0f, suspensionLength);
        rearRight.UpdateWheel(carRigidbody.linearVelocity, 0f, suspensionLength);
    }

    public void ActiveCarController(bool isActive)
    {
        if (isActive)
        {
            isBlock = !isActive;
            carRigidbody.isKinematic = false;
            _camera.SetTarget(transform);
        }
        else
        {
            isBlock = !isActive;
        }
    }

}
