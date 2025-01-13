using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("InputSystem")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action map name reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action name reference")]
    [SerializeField] private string movementActionName = "Movement";
    //[SerializeField] private string jumpActionName = "Jump";
    [SerializeField] private string sprintActionName = "Sprint";
    [SerializeField] private string attackActionName = "Attack";
    [SerializeField] private string aimActionName = "Aim";

    private InputAction moveAction;
    //private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction attackAction;
    private InputAction aimAction;

    public Vector2 moveInput { get; private set; }

    public float sprintValue { get; private set; }

    public bool jumpTriggered { get; private set; }

    public bool attackTriggered { get; private set; }

    public bool aimTriggered { get; private set; }  

    public static InputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(movementActionName);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprintActionName);
        //jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jumpActionName);
        attackAction = playerControls.FindActionMap(actionMapName).FindAction(attackActionName);
        aimAction = playerControls.FindActionMap(actionMapName).FindAction(aimActionName);
        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        sprintAction.performed += context => sprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => sprintValue = 0f;

        //jumpAction.performed += context => jumpTriggered = true;
        //jumpAction.canceled += context => jumpTriggered = false;

        attackAction.performed += context => attackTriggered = true;
        attackAction.canceled += context => attackTriggered = false;

        aimAction.performed += context => aimTriggered = true;
        aimAction.canceled += context => aimTriggered = false;


    }

    private void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
        //jumpAction.Enable();    
        attackAction.Enable();
        aimAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        //jumpAction.Disable();
        attackAction.Disable();
        aimAction.Disable();
    }
}
