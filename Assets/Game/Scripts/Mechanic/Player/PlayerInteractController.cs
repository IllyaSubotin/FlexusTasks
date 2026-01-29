using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _player;
    private CarController _currentCarController;
    private CarController _inCarController;
    private Vector3 localOffset;

    private bool isInCar = false;

    private void OnTriggerEnter(Collider other)
    {
        var carController = other.GetComponent<CarController>();
        if(carController != null)
        {
            _currentCarController = carController;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var carController = other.GetComponent<CarController>();
        if(carController != null)
        {
            _currentCarController = null;
        }
    }

    private void Update()
    {
        if (_currentCarController != null && Input.GetKeyDown(KeyCode.E))
        {
            isInCar = true;
            _inCarController = _currentCarController;

            _player.BlockMovement(true);
            _currentCarController.ActiveCarController(true);

            localOffset = _currentCarController.transform.InverseTransformPoint(_player.transform.position);
            
            transform.position = new Vector3( 0, -10, 0 );            
            
        } 
        else if(Input.GetKeyDown(KeyCode.E) && isInCar && _inCarController.carRigidbody.linearVelocity.magnitude < 1f)
        {
            isInCar = false;

            _player.BlockMovement(false);
            _player.SetCameraDefault();
            _inCarController.ActiveCarController(false);

            transform.position = _inCarController.transform.position + localOffset;
        }
    }
}
