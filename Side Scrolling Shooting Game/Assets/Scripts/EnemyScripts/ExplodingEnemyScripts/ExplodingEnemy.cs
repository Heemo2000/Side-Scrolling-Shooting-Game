using UnityEngine;
using UnityEngine.AI;
using Cinemachine;
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
    [Range(0.1f,5.0f)]
    [SerializeField]private float flyingDestinationOffSet = 2.0f;
    [Range(0.5f,50f)]
    [SerializeField]private float flySpeed = 10f;

    [Range(0.5f,5.0f)]
    [SerializeField]private float flyTime = 3.0f;
    [Range(2f,50f)]
    [SerializeField]private float flyingCheckDistance = 10f;
    [SerializeField]private AnimationCurve flyingCurve;
    [SerializeField]private Animator animator;
    [SerializeField]private ParticleSystem explosionEffect;
    [SerializeField]private BarsUI explodeTimerUIPrefab;

    [SerializeField]private CinemachineImpulseSource explodeImpulse;

    [SerializeField]private Transform groundCheck;

    [Range(0.05f,0.3f)]
    [SerializeField]private float groundCheckDistance = 0.1f;

    [Range(5f,50f)]
    [SerializeField]private float damage = 20f;
    public float ChaseSpeed { get => chaseSpeed;}
    public float MaxPatrolDistance { get => chaseDistance;}
    public float ExplodeDistance { get => explodeDistance;}
    public float ExplodeTime { get => explodeTime;}
    public NavMeshAgent Agent { get => _agent; }
    public float FlySpeed { get => flySpeed; }
    public Animator Animator { get => animator;}
    public ParticleSystem ExplosionEffect { get => explosionEffect;}
    public float FlyingDestinationOffSet { get => flyingDestinationOffSet; }
    public AnimationCurve FlyingCurve { get => flyingCurve; }
    public float FlyTime { get => flyTime; }
    public bool IsFlying { get => _isFlying; set => _isFlying = value; }
    public BarsUI ExplodeTimerUI { get => _explodeTimerUI; }
    public CinemachineImpulseSource ExplodeImpulse { get => explodeImpulse; }
    public float Damage { get => damage; }

    private StateMachine _enemyStateMachine;

    private NavMeshAgent _agent;
    private ExplodingEnemyChaseState _chaseState;
    private ExplodingEnemyFlyingState _flyingState;
    private ExplodingEnemyExplodeState _explodeState;

    private BarsUI _explodeTimerUI;

    private bool _isFlying = false;

    protected override void Awake() 
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _enemyStateMachine = new StateMachine();

        _chaseState = new ExplodingEnemyChaseState(this);
        _flyingState = new ExplodingEnemyFlyingState(this);
        _explodeState = new ExplodingEnemyExplodeState(this);

        _enemyStateMachine.AddTransition(_chaseState,_flyingState,()=> Utility.CheckDistance(transform.position,Target.position,flyingCheckDistance) && IsGrounded());
        _enemyStateMachine.AddTransition(_flyingState,_explodeState,()=> _isFlying == false);
        _enemyStateMachine.AddTransition(_chaseState,_explodeState,()=> Utility.CheckDistance(transform.position,Target.position,explodeDistance));
    }

    protected override void Start() {
        base.Start();
        _explodeTimerUI = Instantiate(explodeTimerUIPrefab,transform.position,Quaternion.identity);
        _explodeTimerUI.FollowTarget = transform;
        _explodeTimerUI.gameObject.SetActive(false);
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

    private bool IsGrounded()
    {
        return Physics.Raycast(groundCheck.position,Vector3.down,groundCheckDistance,~(1 << gameObject.layer));
    }

    private void OnDestroy() 
    {
        Destroy(_explodeTimerUI.gameObject);    
    }
}
