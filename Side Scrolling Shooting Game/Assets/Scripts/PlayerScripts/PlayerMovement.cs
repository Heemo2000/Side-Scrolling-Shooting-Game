using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
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

    [Min(0f)]
    [SerializeField]private float jumpHeight = 10f;

    [Min(0f)]
    [SerializeField]private float fallMultiplier = 1.5f;
    [SerializeField]private Animator playerAnimator;
    

    private CharacterController _controller;
    private float _gravity;
    private float _velocityY;

    private float _initialJumpVelocity;
    private float _currentRotation = 0f;
    
    private float _xInput;

    private float _smoothedXInput;
    private PlayerInput _playerInput;
    private Plane _plane;
    private Vector3 _mouseWorldPos;

    private HumanAimHandler _aimHandler;

    public Vector3 MouseWorldPos { get => _mouseWorldPos; }
    void Awake() {
         _controller = GetComponent<CharacterController>();
         _controller.detectCollisions = false;
         
         _playerInput = GetComponent<PlayerInput>();
         _aimHandler = GetComponent<HumanAimHandler>();

         _plane = new Plane(Vector3.forward,transform.position);
         _currentRotation = 0f;
         _velocityY = 0f;
    }

    private void LateUpdate() 
    {
        _aimHandler.SetAimPosition(_mouseWorldPos);    
    }

    private void FixedUpdate() 
    {
        HandleMovement(); 
    }

    public void OnXInput(InputAction.CallbackContext context)
    {
        _xInput = context.ReadValue<Vector2>().x;   
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        Jump();   
    }

    public void OnMousePositionInput(InputAction.CallbackContext context)
    {
        _plane.SetNormalAndPosition(Vector3.forward,transform.position);
        Vector3 mouseOnScreenPos = context.ReadValue<Vector2>();

        Vector3 viewPort = _playerInput.camera.ScreenToViewportPoint(mouseOnScreenPos);

        bool isMousePointerInside = (viewPort.x >= 0f && viewPort.x <= 1f) && (viewPort.y >= 0f && viewPort.y <= 1f);
        
        if(!isMousePointerInside)
        {
            return;
        }

        Ray ray = _playerInput.camera.ScreenPointToRay(mouseOnScreenPos);

        if(_plane.Raycast(ray,out float distance))
        {
            _mouseWorldPos = ray.origin + ray.direction * distance;
        }
    }

    private void Jump()
    {
        
        if(IsGrounded())
        {
            playerAnimator.SetTrigger("JumpTrigger");
            _velocityY += _initialJumpVelocity;
        }
    }

    private void CalculateGravity()
    {
        float halfJumpTime = jumpTime/2f;
        _gravity = (-2f * jumpHeight)/(halfJumpTime * halfJumpTime);
        _initialJumpVelocity = 2f * jumpHeight/jumpTime;
    }

    private void HandleRotation()
    {
        Vector3 mouseDirection = (_mouseWorldPos - transform.position).normalized;
        
        float targetRotation = 0f;
        if(mouseDirection.x >= 0f)
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
        CalculateGravity();
        _smoothedXInput = Mathf.Lerp(_smoothedXInput,_xInput,xInputSpeed * Time.deltaTime);

        Vector3 mouseDirection = (_mouseWorldPos - transform.position).normalized;
        
        float animMoveInput = (Mathf.Sign(mouseDirection.x) == Mathf.Sign(_smoothedXInput)) ? 1f : -1f;

        animMoveInput *= Mathf.Abs(_smoothedXInput);
        
        playerAnimator.SetFloat("MoveInput",animMoveInput);
        HandleRotation();
        
        
        _controller.Move(Vector3.right * _smoothedXInput * moveSpeed * Time.fixedDeltaTime);
        _controller.Move(Vector3.up * _velocityY * Time.fixedDeltaTime);

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

    private bool AlmostOnGround()
    {
        return Physics.Raycast(groundCheck.position,Vector3.down,maxGroundCheckDist,~(1 << gameObject.layer));     
    }
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position,groundCheckDist,~(1 << gameObject.layer));
    }

    private void OnDrawGizmos() 
    {
        if(groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position,groundCheck.position + Vector3.down * groundCheckDist);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position,groundCheck.position + Vector3.down * maxGroundCheckDist);
        }
    }
}
