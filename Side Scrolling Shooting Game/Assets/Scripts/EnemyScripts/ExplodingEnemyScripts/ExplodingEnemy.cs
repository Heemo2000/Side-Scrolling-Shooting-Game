using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ExplodingEnemy : BaseEnemy
{
    [Range(1f,50f)]
    [SerializeField]private float patrolSpeed = 1f;

    [Range(1f,50f)]
    [SerializeField]private float chaseSpeed = 2f;

    [Range(1f,50f)]
    [SerializeField]private float chaseDistance = 5f;

    [Range(1f,10f)]
    [SerializeField]private float explodeDistance = 1f;

    [Range(0.1f,5f)]
    [SerializeField]private float destCheckDistance = 0.5f;

    [Range(0.5f,3.0f)]
    [SerializeField]private float patrolInterval = 1.0f;
    
    [Range(0.5f,5f)]
    [SerializeField]private float explodeTime = 2f;
    [SerializeField]private Transform[] patrolDestinations;
    [SerializeField]private Animator animator;
    [SerializeField]private ParticleSystem explosionEffect;

    public float PatrolSpeed { get => patrolSpeed;}
    public float ChaseSpeed { get => chaseSpeed;}
    public float MaxPatrolDistance { get => chaseDistance;}
    public float ExplodeDistance { get => explodeDistance;}
    public float DestCheckDistance { get => destCheckDistance;}
    public float ExplodeTime { get => explodeTime;}
    public Transform[] PatrolDestinations { get => patrolDestinations;}
    public NavMeshAgent Agent { get => _agent;}
    public float PatrolInterval { get => patrolInterval;}
    public Animator Animator { get => animator;}
    public ParticleSystem ExplosionEffect { get => explosionEffect;}

    private StateMachine _enemyStateMachine;

    private NavMeshAgent _agent;
    private ExplodingEnemyPatrolState _patrolState;
    ExplodingEnemyChaseState _chaseState;
    ExplodingEnemyExplodeState _explodeState;

    private void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _enemyStateMachine = new StateMachine();

        _patrolState = new ExplodingEnemyPatrolState(this);
        _chaseState = new ExplodingEnemyChaseState(this);
        _explodeState = new ExplodingEnemyExplodeState(this);

        _enemyStateMachine.AddTransition(_patrolState,_chaseState,()=> Utility.CheckDistance(transform.position,Target.position,chaseDistance));
        _enemyStateMachine.AddTransition(_chaseState,_patrolState,()=> !Utility.CheckDistance(transform.position,Target.position,chaseDistance));
        _enemyStateMachine.AddTransition(_chaseState,_explodeState,()=> Utility.CheckDistance(transform.position,Target.position,explodeDistance));
    }

    private void Start() {
        _enemyStateMachine.SetState(_patrolState);
    }
    // Update is called once per frame
    void Update()
    {
        HandleBehaviour();
    }

    protected override void HandleBehaviour()
    {
        _enemyStateMachine?.OnUpdate();
    }
}
