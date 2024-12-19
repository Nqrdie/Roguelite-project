using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement variables")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float rotationSpeed;

    // Booleans for the animator
    public Vector3 currentMovement;
    public bool isWalking;
    public bool isWalkingBackward;
    public bool isSprinting;
    public bool isStrafing;
    public int movementDirection;

    private InputHandler inputHandler;

    private CharacterController characterController;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = gameObject.GetComponent<CharacterController>();
        inputHandler = FindObjectOfType<InputHandler>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Calculate movement speed based on if the player is sprinting
        isSprinting = inputHandler.sprintValue > 0 && !isWalkingBackward;
        float speed = walkSpeed * (isSprinting ? sprintMultiplier : 1f);

        // Movement input from player
        Vector3 movementInput = new Vector3(inputHandler.moveInput.x, 0, inputHandler.moveInput.y);
        movementInput.Normalize();

        // Get the forward and right position of the camera while setting the y to 0 since we dont want to rotate on the y-axis
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Calculate the direction based on where the player is going and looking
        Vector3 worldDirection = cameraRight * movementInput.x + cameraForward * movementInput.z;

        // Apply it to a class variable so we can use it in rotation and jumping
        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        currentMovement += Physics.gravity;

        characterController.Move(currentMovement * Time.deltaTime);

        // logic for the animator booleans
        isWalking = movementInput.magnitude > 0.1f && !isSprinting && !isWalkingBackward;
        isWalkingBackward = Vector3.Dot(cameraForward, worldDirection) < -0.5f && !isSprinting && inputHandler.aimTriggered;
        movementDirection = (movementInput.x > 0.1f) ? 1 : (movementInput.x < -0.1f ? -1 : 0);
        isStrafing = isSprinting && !isWalkingBackward && inputHandler.aimTriggered;

        HandleRotation(worldDirection, cameraForward);
    }

    private void HandleRotation(Vector3 worldDirection, Vector3 cameraForward)
    {
        if (currentMovement.magnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputHandler.aimTriggered ? cameraForward : worldDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
