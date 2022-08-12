using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanEnemy : BaseEnemy
{
    [Range(1f,10f)]    
    [SerializeField]private float stopDistance = 3f;
    [Range(0.5f,8.0f)]
    [SerializeField]private float lookOffSetY = 1.0f;
    [Range(5f,30f)]
    [SerializeField]private float shootingCheckDistance = 10f;
    [SerializeField]private Animator animator;
    [SerializeField]private ParticleSystem explosionEffect;

    [SerializeField]private Gun gun;   
    public Animator Animator { get => animator;}
    public ParticleSystem ExplosionEffect { get => explosionEffect;}
    public NavMeshAgent Agent { get => _agent;}
    
    private NavMeshAgent _agent;

    private HumanAimHandler _aimHandler;
    private void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _aimHandler = GetComponent<HumanAimHandler>();
    }

    private void Update() 
    {
        HandleBehaviour();
    }

    private void LateUpdate() 
    {
        _aimHandler.SetAimPosition(Target.position + Vector3.up * lookOffSetY);    
    }
    public override void HandleBehaviour()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        if(Utility.CheckDistance(transform.position,Target.position,stopDistance))
        {
            _agent.enabled = false;
        }
        else
        {
            _agent.enabled = true;
            _agent.SetDestination(Target.position);
        }
    }
    

    private void HandleShooting()
    {
        Vector3 targetLookPosition = Target.position + Vector3.up * lookOffSetY;
        Vector3 direction = (targetLookPosition - transform.position).normalized;
        if(Physics.Raycast(transform.position,direction,shootingCheckDistance,1 << Target.gameObject.layer))
        {
            gun?.Fire();
        }
    }
}
