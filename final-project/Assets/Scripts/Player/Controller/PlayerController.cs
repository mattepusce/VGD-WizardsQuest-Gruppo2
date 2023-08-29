using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public Weapon weapon;
    //PlayerData playerData;

    PlayerInputs playerInputs;
    [SerializeField] CharacterController controller;
    public Animator animator;

    private bool hasAlredyTouchedGround = false;    // utile per la gestione della caduta del player nella Boss Arena

    public string levelName;

    public float playerSpeed = 3f;  // velocità del player

    Vector2 currentMovementInput;   // movimento 2d preso in input
    Vector3 currentMovement;        // movimento attuale in camminata
    Vector3 currentRunMovement;     // movimento attuale in corsa
    Vector3 appliedMovement;        // movimento applicato, a cui viene assegnato currentMovement o currentRunMovement a seconda che il player stia correndo o meno

    Vector3 cameraRelativeMovement;

    bool isMovementPressed;     // true se vengono premuti i tasti associato al movimento
    bool isRunPressed;          // true se viene premuto il tasto associato alla corsa

    bool isWalking = false;
    bool isRunning = false;

    float rotationSpeed = 12f;      // velocità di rotazione 
    float runMultiplier = 3f;       // moltiplicatore della velocità quando il player corre

    // gravity
    float gravity = -9.8f;
    [SerializeField] private float gravityMultiplier = 5;
    float groundedGravity = -0.05f;                         // quando il player è a terra, la gravità viene cambiata
                                                            // per ridurre la possibilità di clip del player con il terreno

    // jump
    bool isJumpPressed = false;     // bool per controllare se il tasto di salto viene premuto
    public float jumpSpeed = 20;
    private float velocity;
    bool isJumping = false;         // bool per controllare se il player sta saltando

    // ----------------------------------------------------------------------------

    private void Awake()
    {
        levelName = SceneManager.GetActiveScene().name;
        if(levelName != "Main Menu")
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        playerInputs = new PlayerInputs();                               // inizializzazione playerInputs
        controller = GetComponent<CharacterController>();           // assegnamento del character controller

        // Callback functions

        playerInputs.Player.Move.started += OnMovementInput;            // started -> istante in cui viene premuto il tasto
        playerInputs.Player.Move.canceled += OnMovementInput;           // cancelled -> istante in cui viene rilasciato il tasto
        playerInputs.Player.Move.performed += OnMovementInput;          // performed -> periodo in cui il tasto è premuto
                                                                        // performed è necessario per poter cambiare direzione senza fermare il player
                                                                        // per questo motivo non è necessario per run, jump e roll
        playerInputs.Player.Run.started += onRun;
        playerInputs.Player.Run.canceled += onRun;
        playerInputs.Player.Jump.started += onJump;
        playerInputs.Player.Jump.canceled += onJump;

        //SavePlayer();

    }

    // ----------------------------------------------------------------------------

    // Lettura degli input dall'input system

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();                                    // leggge il valore inserito per il movimento
                                                                                                // W -> (0,1) / S -> (0,-1) / A -> (-1,0) / D -> (1,0)
                                                                                                // WD -> (0.71,0.71) / WA -> (-0.71,0.71) / SD -> (0.71,-0.71) / SA -> (-0.71,-0.71)

        currentMovement.x = currentMovementInput.x * playerSpeed;                               // aggiorna il valore del movimento per il player che cammina
        currentMovement.z = currentMovementInput.y * playerSpeed;                               // moltiplica l'input per la velocità del player

        currentRunMovement.x = currentMovementInput.x * playerSpeed * runMultiplier;            // aggiorna il valore del movimento per il player che corre
        currentRunMovement.z = currentMovementInput.y * playerSpeed * runMultiplier;            // moltiplica l'input per la velocità del player e per il multiplier della corsa

        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;         // true se viene inserito un input di moviemento
    }

    // legge il valore inserito in input e verifica se è stato premuto il tasto jump
    private void onJump(InputAction.CallbackContext context) { isJumpPressed = context.ReadValueAsButton(); }

    // legge il valore inserito in input e verifica se è stato premuto il tasto run
    private void onRun(InputAction.CallbackContext context) { isRunPressed = context.ReadValueAsButton(); }


    // ----------------------------------------------------------------------------

    void HandleGravity()
    {
        if (controller.isGrounded && velocity < 0f)     // se il player è a terra 
        {
            animator.SetBool("isJumping", false);       // termina l'animazione di salto
            velocity = groundedGravity;                 // setta la velocity a un valore leggermente negativo per stare attaccato al terreno
                                                        // e per evitare clip con esso
        }
        else
        {                                                                   // se invece il player non è a terra
            velocity += gravity * gravityMultiplier * Time.deltaTime;       // aggiorna la velocity sottraendo un multiplo della gravità ad ogni ciclo 
        }                                                                   // (e quindi simulando l'accelerazione di gravità)

        currentMovement.y = velocity;           // aggiornare current movement y con la velocity attuale
        currentRunMovement.y = velocity;        // aggiornare current run movement y con la velocity attuale
    }

    void HandleJump()
    {
        if (!isJumping && controller.isGrounded && isJumpPressed)       // se viene premuto il tasto jump quando il player è a terra
        {
            velocity += jumpSpeed;                      // aggiorna la velocity sommando il valore di jumpSpeed, quindi velocity diventa positiva
            animator.SetBool("isJumping", true);        // avvia l'animazione di salto
            isJumping = true;                           // il player sta saltando
            currentMovement.y = velocity;               // aggiornare current movement y con la velocity attuale, il player salta
            currentRunMovement.y = velocity;            // aggiornare current run movement y con la velocity attuale, il player salta
        }
        else if (isJumping && controller.isGrounded && !isJumpPressed)      // se il player sta saltando, è a terra e non è premuto il tasto jump
        {
            isJumping = false;      // allora il player non sta saltando
        }

        // gestione caduta Boss Arena
        if (controller.isGrounded) hasAlredyTouchedGround = true;
        if (!controller.isGrounded && SceneManager.GetActiveScene().name == "Boss Arena" && !hasAlredyTouchedGround) animator.SetBool("isJumping", true); 
    }

    void HandleAnimation()
    {
        isWalking = animator.GetBool("isWalking");      // true se il player è in animazione di camminata
        isRunning = animator.GetBool("isRunning");      // true se il player è in animazione di corsa

        if (isMovementPressed && !isWalking) animator.SetBool("isWalking", true);           // se viene preso un input di movimento, avvia l'animazione di camminata
        else if (!isMovementPressed && isWalking) animator.SetBool("isWalking", false);     // se non viene più preso un input di movimento, termina l'animazione di camminata

        if (isMovementPressed && isRunPressed && !isRunning) animator.SetBool("isRunning", true);           // se viene preso un input di movimento e viene premuto il tasto corsa, avvia l'animazione di corsa
        else if ((!isMovementPressed || !isRunPressed) && isRunning) animator.SetBool("isRunning", false);  // se viene terminato l'input di movimento o l'input di corsa, termina l'animazione di corsa      
    }

    void HandleRotation()
    {
        Vector3 newLookDirection;                           // vettore in cui viene salvata la direzione in cui dovrà guardare il player
        Quaternion currentRotation = transform.rotation;    // viene salvata la rotazione corrente del player

        // positionToLookAt dovrà essere in relazione a cameraRelativeMovement, ovvero il movimento applicato in input convertito in camera space
        newLookDirection.x = cameraRelativeMovement.x;
        newLookDirection.y = 0f;
        newLookDirection.z = cameraRelativeMovement.z;

        if (isMovementPressed)      // se viene preso un input di movimento
        {
            Quaternion targetRotation = Quaternion.LookRotation(newLookDirection);                                      // calcola una rotazione di arrivo relativa a positionToLookAt
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);     // applica la rotazione al player tramite interpolazione sferica
                                                                                                                        // quindi passa dalla currentRotation alla targetRotation a velocità rotationSpeed
                                                                                                                        // interpolando le fasi intermedie
        }
    }

    Vector3 ConvertToCameraSpace(Vector3 worldSpaceVector)
    {
        float previousYValue = worldSpaceVector.y;      // salvataggio del valore y del vettore passato come parametro

        // vettori forward e right della camera
        Vector3 cameraForward = Camera.main.transform.forward;      // cameraForward è il vettore che punta in direzione avanti rispetto alla camera nel sistema di coordinate mondo
        Vector3 cameraRight = Camera.main.transform.right;          // cameraRight è il vettore che punta in direzione destra rispetto alla camera nel sistema di coordinate mondo

        // permette di ignorare gli angoli verticali della camera nei movimenti
        cameraForward.y = 0;
        cameraRight.y = 0;

        // ruota la X e la Z di vectorToRotate allo spazio camera 
        // Se il player è fermo, il vettore passato come parametro sarà (0,0,0) (ovvero appliedMovement), quindi anche cameraForwardZProduct e cameraRightXProduct varranno (0,0,0)
        Vector3 cameraForwardProduct = worldSpaceVector.z * cameraForward;      // viene moltiplicato cameraForward per la componente z del vettore passato come parametro
        Vector3 cameraRightProduct = worldSpaceVector.x * cameraRight;          // viene moltiplicato cameraRight per la componente x del vettore passato come parametro
                                                                                // Se il player si muove in avanti, questa parte di codice permette di stabilire che avanti non è più in relazione al world space,
                                                                                // ma è in relazione al camera space, infatti vectorToRotate, che essendo appliedMovement indica il movimento applicato al player,
                                                                                // ora verrà applicato in relazione al camera space

        // la somma dei due prodotti è il vector3 dello spazio camera
        Vector3 vectorRotatedToCameraSpace = cameraForwardProduct + cameraRightProduct;     // somma i valori dei due vettori per ottenerne uno finale ruotato in relazione al camera space
        vectorRotatedToCameraSpace.y = previousYValue;                                      // viene ripristinato il valore iniziale di y, ovvero l'inclinazione verticale della camera
        return vectorRotatedToCameraSpace;                                                  // viene ritornato il vettore ruotato in funzione del camera space
    }


    // ----------------------------------------------------------------------------

    private void Update()
    {
        // bool utili per sapere se le animazioni di attacco o scudo sono attive
        bool isAttacking = getState("LightSword") || getState("HeavySword") || getState("LightAxe") || getState("HeavyAxe") || getState("LightMace") || getState("HeavyMace");
        bool shield = getState("Shield");
        bool isDying = getState("Morto di morte");

        if (isAttacking || shield|| isDying) OnDisable();     // se le animazioni di attacco o scudo sono attive, disattiva gli input       
        else OnEnable();

        HandleRotation();       // gestione della rotazione del player
        HandleAnimation();      // gestione delle animazioni
        HandleGravity();        // gestione della gravità
        HandleJump();           // gestione dei salti


        if (isRunPressed) appliedMovement = currentRunMovement;     // se il player sta correndo utilizza currentRunMovement
        else appliedMovement = currentMovement;                     // altrimenti utilizza currentMovement

        cameraRelativeMovement = ConvertToCameraSpace(appliedMovement);     // converte appliedMovement dal world space al camera space 
        controller.Move(cameraRelativeMovement * Time.deltaTime);           // applica il movimento sul nuovo vettore relativo al camera space
        // 


    }


    // ----------------------------------------------------------------------------

    private void OnEnable() { playerInputs.Player.Enable(); }       // abilitazione degli input
    private void OnDisable() { playerInputs.Player.Disable(); }     // disabilitazione degli input

    // getState restituisce true se l'animazione passata come parametro è attiva
    private bool getState(string animationName) { return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName); }

}
