using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("InputSystem")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action map name reference")]
    [SerializeField] private string actionMapName = "Player";

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction attackAction;

    public Vector2 moveInput { get; private set; }

    public float sprintValue { get; private set; }

    public bool jumpTriggered { get; private set; }

    public bool attackTriggered { get; private set; }

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

        moveAction = playerControls.FindActionMap(actionMapName).FindAction("Movement");
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction("Sprint");
        //jumpAction = playerControls.FindActionMap(actionMapName).FindAction("Jump");
        //attackAction = playerControls.FindActionMap(actionMapName).FindAction("Attack");
        RegisterInputActions();
    }

    private void RegisterInputActions()
    {
        moveAction.performed += context => moveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => moveInput = Vector2.zero;

        sprintAction.performed += context => sprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => sprintValue = 0f;

        //jumpAction.performed += context => jumpTriggered = true;
        //jumpAction.performed += context => jumpTriggered = false;

        //attackAction.performed += context => attackTriggered = true;
       // attackAction.performed += context => attackTriggered = false;


    }

    private void OnEnable()
    {
        moveAction.Enable();
        sprintAction.Enable();
        //jumpAction.Enable();    
        //attackAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        sprintAction.Disable();
        //jumpAction.Disable();
        //attackAction.Disable();
    }
}
