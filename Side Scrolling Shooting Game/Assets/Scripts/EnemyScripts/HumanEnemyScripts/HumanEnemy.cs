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
    public Animator HumanAnimator { get => animator;}
    public ParticleSystem ExplosionEffect { get => explosionEffect;}
    public NavMeshAgent Agent { get => _agent;}
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    private NavMeshAgent _agent;
    private GenericAimHandler _aimHandler;

    private StateMachine _enemyStateMachine;

    private HumanEnemyChaseState _chaseState;
    private HumanEnemyStopState _stopState;

    private Health _health;
    private float _currentMoveAnimSpeed = 0;

    private bool _shouldStop = false;
    protected override void Awake() 
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _aimHandler = GetComponent<GenericAimHandler>();
        _enemyStateMachine = new StateMachine();
        
        _chaseState = new HumanEnemyChaseState(this);
        _stopState = new HumanEnemyStopState(this);

        _enemyStateMachine.AddTransition(_chaseState,_stopState,() => Utility.CheckDistance(transform.position,Target.position,stopDistance) || _shouldStop);
        _enemyStateMachine.AddTransition(_stopState,_chaseState,()=> !Utility.CheckDistance(transform.position,Target.position,stopDistance) || !_shouldStop);
    
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

        _aimHandler.SetAimPosition(Target.position + Vector3.up * lookOffSetY);
        float targetMoveAnimSpeed = (_agent.enabled) ? _agent.velocity.normalized.magnitude : 0f;
        _currentMoveAnimSpeed = Mathf.Lerp(_currentMoveAnimSpeed,targetMoveAnimSpeed,moveSpeed * Time.deltaTime);
        animator.SetFloat(StringHolder.MoveInputAnimParam,_currentMoveAnimSpeed);
    }

    private void HandleShooting()
    {
        Vector3 targetLookPosition = Target.position + Vector3.up * lookOffSetY;
        Vector3 direction = (targetLookPosition - eye.position).normalized;
        int shootCheckMaskValue = ~(1 << Target.gameObject.layer | collisionIgnoreLayerMask.value);
        if(!Physics.Raycast(eye.position,direction,shootingCheckDistance,shootCheckMaskValue))
        {
            gun?.Fire();
        }
    }

    private void HandleFrontSide()
    {
        int requiredMaskValue = ~(collisionIgnoreLayerMask.value);

        Ray ray = new Ray(transform.position,transform.forward);      
        _shouldStop = Physics.SphereCast(ray,0.1f,frontSideCheckDistance,requiredMaskValue);
        if(_shouldStop)
        {
            Debug.Log("Now, stop.");
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + transform.forward * frontSideCheckDistance);    
    }
}
