using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerInputs playerInputs;
    CharacterController controller;
    public Animator animator;


    Vector2 currentMovementInput;

    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;

    bool isMovementPressed;
    bool isRunPressed;

    float rotationFactorPerFrame = 12f;
    float runMultiplier = 3f;

    float gravity = -9.8f;
    float groundedGravity = -0.05f;

    // jumping
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 3f;
    float maxJumpTime = 0.75f;
    bool requireNewJumpPress = false;
    bool isJumping = false;

    PlayerBaseState currentState;
    PlayerStateFactory states;

    public PlayerBaseState CurrentState { get { return currentState; } set { currentState = value; } }       // getter & setter
    public CharacterController CharacterController { get { return controller; } set { controller = value; } }
    public Animator Animator { get { return animator; } }
    public float InitialJumpVelocity { get { return initialJumpVelocity; } }
    public bool IsJumpPressed { get { return isJumpPressed; } set { isJumpPressed = value; } }      // getter and setter
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    public float CurrentMovementX { get { return currentMovement.x; } set { currentMovement.x = value; } }
    public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; } }
    public float CurrentMovementZ { get { return currentMovement.z; } set { currentMovement.z = value; } }
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; } }
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; } }
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; } }
    public float Gravity { get { return gravity; } }
    public float GroundedGravity { get { return groundedGravity; } }
    public bool RequireNewJumpPress { get { return requireNewJumpPress; } set { requireNewJumpPress = value; } }
    public bool IsMovementPressed { get { return isMovementPressed; } }
    public bool IsRunPressed { get { return isRunPressed; } }
    public float RunMultiplier { get { return runMultiplier; } }
    



    private void Awake()
    {
        playerInputs = new PlayerInputs();
        controller = GetComponent<CharacterController>();

        states = new PlayerStateFactory(this);
        currentState = states.Grounded();
        currentState.EnterState();

        playerInputs.Player.Move.started += OnMovementInput;
        playerInputs.Player.Move.canceled += OnMovementInput;
        playerInputs.Player.Move.performed += OnMovementInput;
        playerInputs.Player.Run.started += onRun;
        playerInputs.Player.Run.canceled += onRun;
        playerInputs.Player.Jump.started += onJump;
        playerInputs.Player.Jump.canceled += onJump;

        setupJumpVariables();
    }

    private void Update()
    {
        HandleRotation();
        currentState.UpdateStates();
        controller.Move(appliedMovement * Time.deltaTime);
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }

    }


    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }


    private void onJump(InputAction.CallbackContext context)
    {
        //Debug.Log(isJumpPressed);
        isJumpPressed = context.ReadValueAsButton();
        requireNewJumpPress = false;                         // per evitare salti infiniti tenendo premuto Space
    }

    private void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void OnEnable()
    {
        playerInputs.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Player.Disable();
    }


}
