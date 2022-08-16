using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ExplodingEnemy : BaseEnemy
{
    
    [Range(1f,50f)]
    [SerializeField]private float chaseSpeed = 2f;

    [Range(1f,50f)]
    [SerializeField]private float chaseDistance = 5f;

    [Range(1f,10f)]
    [SerializeField]private float explodeDistance = 1f;
    [Range(0.5f,5f)]
    [SerializeField]private float explodeTime = 2f;

    [Range(0.2f,5.0f)]
    [SerializeField]private float flyTime = 1.0f;
    [Range(0.1f,5.0f)]
    [SerializeField]private float flyingDestinationOffSet = 2.0f;
    [Range(0.5f,50f)]
    [SerializeField]private float flySpeed = 10f;

    [Range(2f,50f)]
    [SerializeField]private float flyingCheckDistance = 10f;
    [SerializeField]private Animator animator;
    [SerializeField]private ParticleSystem explosionEffect;

    public float ChaseSpeed { get => chaseSpeed;}
    public float MaxPatrolDistance { get => chaseDistance;}
    public float ExplodeDistance { get => explodeDistance;}
    public float ExplodeTime { get => explodeTime;}
    public NavMeshAgent Agent { get => _agent; }
    public float FlyTime { get => flyTime; }
    public float FlySpeed { get => flySpeed; }
    public Animator Animator { get => animator;}
    public ParticleSystem ExplosionEffect { get => explosionEffect;}
    public Health Health { get => _health; }
    public float FlyingDestinationOffSet { get => flyingDestinationOffSet; }

    private StateMachine _enemyStateMachine;

    private NavMeshAgent _agent;
    private Health _health;
    private ExplodingEnemyChaseState _chaseState;
    private ExplodingEnemyFlyingState _flyingState;
    private ExplodingEnemyExplodeState _explodeState;

    private void Awake() 
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _enemyStateMachine = new StateMachine();

        _chaseState = new ExplodingEnemyChaseState(this);
        _flyingState = new ExplodingEnemyFlyingState(this);
        _explodeState = new ExplodingEnemyExplodeState(this);

        _enemyStateMachine.AddTransition(_chaseState,_flyingState,()=> Utility.CheckDistance(transform.position,Target.position,flyingCheckDistance));
        _enemyStateMachine.AddTransition(_flyingState,_explodeState,()=> Utility.CheckDistance(transform.position,Target.position,explodeDistance));
        _enemyStateMachine.AddTransition(_chaseState,_explodeState,()=> Utility.CheckDistance(transform.position,Target.position,explodeDistance));
    }

    private void Start() {
        _enemyStateMachine.SetState(_chaseState);
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
