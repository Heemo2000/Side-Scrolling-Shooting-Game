using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanEnemy : BaseEnemy
{
    [SerializeField]private Transform eye;

    [Range(0.5f,3.0f)]
    [SerializeField]private float frontSideCheckDistance = 2f;
    [Range(5f,30f)]
    [SerializeField]private float shootingCheckDistance = 10f;

    [Range(1f,5f)]
    [SerializeField]private float groundCheckDistance = 5f;
    [SerializeField]private LayerMask collisionIgnoreLayerMask;

    [SerializeField]private LayerMask offMeshLinkLayerMask;
    [SerializeField]private LayerMask groundLayerMask;
    [SerializeField]private Transform groundCheck;

    [Range(1f,10f)]
    [SerializeField]private float moveSpeed = 5f;
    [Range(1f,10f)]    
    [SerializeField]private float stopDistance = 3f;
    
    [Range(0.5f,8.0f)]
    [SerializeField]private float lookOffSetY = 1.0f;
    
    [SerializeField]private Animator animator;
    [SerializeField]private ParticleSystem explosionEffect;
    [SerializeField]private Gun gun;   
    public Animator Animator { get => animator;}
    public ParticleSystem ExplosionEffect { get => explosionEffect;}
    public NavMeshAgent Agent { get => _agent;}
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private NavMeshAgent _agent;
    private HumanAimHandler _aimHandler;

    private StateMachine _enemyStateMachine;

    private HumanEnemyChaseState _chaseState;
    private HumanEnemyStopState _stopState;
    private float _currentMoveAnimSpeed = 0;

    private bool _shouldStop = false;

    private bool _isJumping = false;
    private void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _aimHandler = GetComponent<HumanAimHandler>();
        _enemyStateMachine = new StateMachine();
        
        _chaseState = new HumanEnemyChaseState(this);
        _stopState = new HumanEnemyStopState(this);

        _enemyStateMachine.AddTransition(_chaseState,_stopState,() => Utility.CheckDistance(transform.position,Target.position,stopDistance) 
                                         || (_shouldStop == true));
        _enemyStateMachine.AddTransition(_stopState,_chaseState,()=> !Utility.CheckDistance(transform.position,Target.position,stopDistance)
                                         || (_shouldStop == false));
    
        _enemyStateMachine?.SetState(_chaseState);
    }

    private void Update() 
    {
        HandleBehaviour();
    }

    protected override void HandleBehaviour()
    {
        _enemyStateMachine?.OnUpdate();

        HandleFrontSide();
        HandleShooting();
        HandleAnimation();

        _aimHandler.SetAimPosition(Target.position + Vector3.up * lookOffSetY);
    }
    
    private void HandleAnimation()
    {
        float targetMoveAnimSpeed = (_agent.enabled) ? _agent.velocity.normalized.magnitude : 0f;
        _currentMoveAnimSpeed = Mathf.Lerp(_currentMoveAnimSpeed,targetMoveAnimSpeed,moveSpeed * Time.deltaTime);
        animator.SetFloat(StringHolder.MoveInputAnimParam,_currentMoveAnimSpeed);

        /*
        if(!_isJumping && Physics.Raycast(transform.position,transform.forward,out RaycastHit hit,frontSideCheckDistance,offMeshLinkLayerMask.value))
        {
            _isJumping = true;
            Debug.Log("Trigger Call on enemy jump up");
            animator.SetTrigger(StringHolder.JumpTriggerAnimParameter);
            //animator.ResetTrigger(StringHolder.JumpTriggerAnimParameter);
        }
        */
        HandleFallingAnimation();
    }

    private void HandleShooting()
    {
        Vector3 targetLookPosition = Target.position + Vector3.up * lookOffSetY;
        Vector3 direction = (targetLookPosition - eye.position).normalized;
        int shootCheckMaskValue = ~(1 << Target.gameObject.layer);
        if(!Physics.Raycast(eye.position,direction,shootingCheckDistance,shootCheckMaskValue))
        {
            gun?.Fire();
        }
    }

    private void HandleFallingAnimation()
    {
        bool almostOnGround = Physics.CheckSphere(groundCheck.position,groundCheckDistance,groundLayerMask.value);
        animator.SetBool(StringHolder.IsFallingAnimParameter,!almostOnGround);
    }

    private void HandleFrontSide()
    {
        RaycastHit hit;

        int requiredMaskValue = ~(1 << gameObject.layer | collisionIgnoreLayerMask.value);
        _shouldStop = Physics.Raycast(transform.position,transform.forward,out hit,frontSideCheckDistance,
                      requiredMaskValue);
    }

    private void OnTriggerEnter(Collider other) 
    {
        
        int collidedLayerMask = 1 << other.gameObject.layer;
        if(collidedLayerMask == offMeshLinkLayerMask.value)
        {
            animator.SetTrigger(StringHolder.JumpTriggerAnimParameter);
        } 
    }
}
