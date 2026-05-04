using UnityEngine;
using UnityEngine.InputSystem;
public class Character : MonoBehaviour
    {
    private bool isJumping = false;
    private float jumpCooldownTimer;
    private CharacterController controller;
    private InputAction moveAction;
    private InputAction jumpAction;
    [SerializeField]
    private float jumpCooldown;
    //We set gravity lower than in real live as it is more fun!
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float characterSpeed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float dampening;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField] private LayerMask platformLayerMask;
    private Animator animator;
    private Vector3 characterMovement;
    private Vector3 jumpVelocity;
    private Vector3 characterGravity;
    
    private MovingPlatform currentPlatform;

    void Start()
    {
        this.controller = this.GetComponent<CharacterController>();
        this.animator = this.GetComponent<Animator>();
        this.moveAction = InputSystem.actions.FindAction("Move");
        this.jumpAction = InputSystem.actions.FindAction("Jump");
        this.jumpCooldownTimer = 0.0f;
    }

    private void SetAnimationState(Vector2 inputMovement)
    {
        this.animator.SetFloat("RunningSpeed", inputMovement.magnitude);
        this.animator.SetBool("isJumping", this.isJumping);
    }
    void HandleJumping()
    {
        if (this.controller.isGrounded && this.isJumping && this.jumpCooldownTimer <= 0.0f) {
            this.jumpVelocity = Vector3.zero;
            this.isJumping = false;
        }

        if (this.controller.isGrounded && !this.isJumping && this.jumpAction.WasPressedThisFrame()) {
            this.characterGravity = Vector3.zero;
            this.jumpVelocity = Vector3.zero;
            this.jumpVelocity.y = this.jumpSpeed;
            this.jumpCooldownTimer = this.jumpCooldown;
            this.isJumping = true;
            this.currentPlatform = null;
        }

        if (this.jumpVelocity.y > 0.0f) {
            this.jumpVelocity.y -= Time.fixedDeltaTime;
        } else {
            this.jumpVelocity = Vector3.zero;
        }

        this.jumpCooldownTimer -= Time.fixedDeltaTime;
    }

    private Vector3 GetPlatformVelocity()
    {
        Vector3 platformVelocity = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 1.0f, platformLayerMask))
        {
            MovingPlatform platform = hit.collider.GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platformVelocity = platform.Velocity;
                this.currentPlatform = platform;
            }
        }
        else
        {
            this.currentPlatform = null;
        }
        return platformVelocity;
    }

    void FixedUpdate()
    {
        this.HandleJumping();
        var inputMovement = this.moveAction.ReadValue<Vector2>();
        var inputRightDirection = this.cameraTransform.right;
        var inputForwardDirection = this.cameraTransform.forward;
        this.SetAnimationState(inputMovement);
        
        inputRightDirection.y = 0.0f;
        inputForwardDirection.y = 0.0f;
        inputRightDirection.Normalize();
        inputForwardDirection.Normalize();

        var platformVelocity = this.GetPlatformVelocity();

        //Since we do not use the physics system, we have to simulate gravity ourselves
        if(this.controller.isGrounded) {
            this.characterGravity.y = 0.0f;
        }

        this.characterGravity.y += this.gravity * Time.fixedDeltaTime;

        this.characterMovement += this.characterGravity * Time.fixedDeltaTime;
        this.characterMovement += this.jumpVelocity * Time.fixedDeltaTime;
        this.characterMovement += inputRightDirection * inputMovement.x * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement += inputForwardDirection * inputMovement.y * this.characterSpeed * Time.fixedDeltaTime;
        this.characterMovement *= (1 - this.dampening);
        Vector3 characterForward = this.characterMovement;
        characterForward.y = 0.0f;
        
        if (characterForward.sqrMagnitude > 0.0f && characterForward != Vector3.zero) {
            this.transform.forward = characterForward.normalized;
        }

        var combinedMovement = this.characterMovement;
        if (/*this.controller.isGrounded && */this.currentPlatform != null)
        {
            combinedMovement += platformVelocity * Time.fixedDeltaTime;
        }
        this.controller.Move(combinedMovement);
    }
}