using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const float MaxAimCircleSize = 5f;
    private const float AimCheckForwardOffSet = 2.5f;
    
    [Min(1f)]
    [SerializeField]private float xInputSpeed = 5f;
    [Min(0f)]
    [SerializeField]private float moveSpeed = 5f;
    [Min(5f)]
    [SerializeField]private float rotationSpeed = 90f;
    [Min(0f)]
    [SerializeField]private float jumpTime = 0.5f;

    [SerializeField]private Transform groundCheck;
    
    [Range(0.05f,0.5f)]
    [SerializeField]private float groundCheckDist = 0.1f;

    [Range(0.1f,2.5f)]
    [SerializeField]private float maxGroundCheckDist = 1.0f;
    [SerializeField]private float aimCheckRadius = 2f;
    [SerializeField]private float aimCheckMaxDistance = 10f;
    [Min(0f)]
    [SerializeField]private float jumpHeight = 10f;

    [SerializeField]private LayerMask groundCheckIgnore;

    [Min(0f)]
    [SerializeField]private float fallMultiplier = 1.5f;
    [SerializeField]private Animator playerAnimator;
    [SerializeField]private LayerMask aimMask;
    [SerializeField]private Transform firePoint;
    private CharacterController _controller;
    private float _gravity;
    private float _velocityY;

    private float _initialJumpVelocity;
    private float _currentRotation = 0f;
    
    private float _xInput;

    private float _smoothedXInput;
    private PlayerInput _playerInput;
    
    private Vector3 _aimPos;

    private GenericAimHandler _aimHandler;
    private float _lockedZPosition = 0f;

    public Vector3 MouseWorldPos { get => _aimPos; }
    public GenericAimHandler AimHandler { get => _aimHandler; }

    void Awake() {
         _controller = GetComponent<CharacterController>();
         _controller.detectCollisions = false;
         
         _playerInput = GetComponent<PlayerInput>();
         _aimHandler = GetComponent<GenericAimHandler>();

         
         _currentRotation = 0f;
         _velocityY = 0f;
    }
    private void Start() 
    {
        _lockedZPosition = transform.position.z;  
    }

    private void Update() 
    {
        if(PauseController.Instance.IsGamePaused || GameManager.Instance.IsGameEnded)
        {
            playerAnimator.enabled = false;
            return;
        }
        else if(PauseController.Instance.IsGamePaused == false)
        {
            playerAnimator.enabled = true;
        }    
    }

    private void FixedUpdate() 
    {
        if(GameManager.Instance.IsGameEnded)
        {
            return;
        }
        HandleMovement(); 
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(PauseController.Instance.IsGamePaused)
        {
            return;
        }
        if(context.performed)
        {
            Jump();
        }
           
    }

    public void OnAimInput(InputAction.CallbackContext context)
    {
        Vector3 aimDirection = context.ReadValue<Vector2>();
        
        if(aimDirection.x >= 0.5f)
        {
            _xInput = 1f;
        }
        else if(aimDirection.x <= -0.5f)
        {
            _xInput = -1f;
        }
        else 
        {
            _xInput = 0f;
        }
        if(Mathf.Approximately(aimDirection.magnitude,0f))
        {
            return;
        }
        Vector3 estimatedAimPosition = transform.position + new Vector3(aimDirection.x, aimDirection.y) * MaxAimCircleSize;
        if(Physics.SphereCast(firePoint.position,aimCheckRadius,aimDirection,out RaycastHit hit,aimCheckMaxDistance,aimMask.value))
        {
            estimatedAimPosition = hit.transform.position;
        }
        _aimHandler.SetAimPosition(estimatedAimPosition);
        _aimPos = estimatedAimPosition;
    }
    

    private void Jump()
    {
        if(IsGrounded())
        {
            playerAnimator.SetTrigger(StringHolder.JumpTriggerAnimParameter);
            _velocityY += _initialJumpVelocity;
        }
    }

    private void CalculateParameters()
    {
        float halfJumpTime = jumpTime/2f;
        _gravity = (-2f * jumpHeight)/(halfJumpTime * halfJumpTime);
        _initialJumpVelocity = 2f * jumpHeight/halfJumpTime;
    }

    
    
    private void HandleRotation()
    {
        Vector3 direction = (_aimPos - transform.position).normalized;
        
        float targetRotation = 0f;
        if(direction.x >= 0f)
        {
             targetRotation = 0f;
        }
        else
        {
            targetRotation = -180f;
        }

        _currentRotation = Mathf.SmoothStep(_currentRotation,targetRotation,rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.AngleAxis(_currentRotation,Vector3.up);
    }
    
    private void HandleMovement()
    {
        CalculateParameters();
        _smoothedXInput = Mathf.Lerp(_smoothedXInput,_xInput,xInputSpeed * Time.deltaTime);

        Vector3 direction = (_aimPos - transform.position).normalized;
        
        float animMoveInput = (Mathf.Sign(direction.x) == Mathf.Sign(_smoothedXInput)) ? 1f : -1f;

        animMoveInput *= Mathf.Abs(_smoothedXInput);
        
        playerAnimator.SetFloat("MoveInput",animMoveInput);

        HandleRotation();
        
        _controller.Move(Vector3.right * _smoothedXInput * moveSpeed * Time.fixedDeltaTime);
        _controller.Move(Vector3.up * _velocityY * Time.fixedDeltaTime);
        LockPositionInZ();

        HandleGravity();
    }

    private void HandleGravity()
    {
        
        bool isFalling = _velocityY < 0.0f;

        if(!IsGrounded())
        {
            float currentVelocityY = _velocityY;
            float currentGravity = _gravity;
            
            if(isFalling)
            {
                playerAnimator.SetBool("IsFalling",true);
                currentGravity *= fallMultiplier;
            }
            float newVelocityY = currentVelocityY + (currentGravity * Time.fixedDeltaTime);
            float nextVelocityY = (currentVelocityY + newVelocityY) * 0.5f;
            _velocityY = nextVelocityY;

        }
        else
        {
            _velocityY = 0f;
        }

        if(AlmostOnGround() && isFalling)
        {    
            playerAnimator.SetBool("IsFalling",false);
        }
    }

    private void LockPositionInZ()
    {
        Vector3 lockedPosition = transform.position;
        lockedPosition.z = _lockedZPosition;
        transform.position = lockedPosition;
    }
    private bool AlmostOnGround()
    {
        return Physics.CheckSphere(groundCheck.position,maxGroundCheckDist,~groundCheckIgnore.value);     
    }
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position,groundCheckDist,~groundCheckIgnore.value);
    }

    private void OnDrawGizmos() 
    {
        if(groundCheck != null)
        {
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position,groundCheck.position + Vector3.down * groundCheckDist);
            
            /*
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position,groundCheck.position + Vector3.down * maxGroundCheckDist);
            */
        }
    }
}
