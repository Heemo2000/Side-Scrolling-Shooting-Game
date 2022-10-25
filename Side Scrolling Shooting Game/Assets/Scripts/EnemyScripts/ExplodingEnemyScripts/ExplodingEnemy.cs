using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class ExplodingEnemy : BaseEnemy
{
    [Range(1f,50f)]
    [SerializeField]private float chaseSpeed = 2f;

    [Range(1f,4f)]
    [SerializeField]private float explodeDistance = 1f;
    
    [Range(0.1f,1f)]
    [SerializeField]private float explodeTime = 1f;
    [SerializeField]private Animator animator;
    [SerializeField]private ParticleSystem explosionEffect;
    [SerializeField]private BarsUI explodeTimerUIPrefab;
    [SerializeField]private CinemachineImpulseSource explodeImpulse;
    [Range(5f,50f)]
    [SerializeField]private float damage = 20f;
    [SerializeField]private Transform frontCheck;
    [SerializeField]private LayerMask rayCastIgnore;

    [Min(1f)]
    [SerializeField]private float maxFrontCheckDistance = 4f;

    
    public float ChaseSpeed { get => chaseSpeed;}
    public float ExplodeDistance { get => explodeDistance;}
    public float ExplodeTime { get => explodeTime;}
    public NavMeshAgent Agent { get => _agent; }
    public Animator Animator { get => animator;}
    public ParticleSystem ExplosionEffect { get => explosionEffect;}
    public BarsUI ExplodeTimerUI { get => _explodeTimerUI; }
    public CinemachineImpulseSource ExplodeImpulse { get => explodeImpulse; }
    public float Damage { get => damage; }
    public Transform FrontCheck { get => frontCheck;}
    public LayerMask RayCastIgnore { get => rayCastIgnore; }
    public float MaxFrontCheckDistance { get => maxFrontCheckDistance; }
    
    private StateMachine _enemyStateMachine;

    private NavMeshAgent _agent;
    private ExplodingEnemyChaseState _chaseState;
    private ExplodingEnemyExplodeState _explodeState;

    private BarsUI _explodeTimerUI;

    protected override void Awake() 
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _enemyStateMachine = new StateMachine();

        _chaseState = new ExplodingEnemyChaseState(this);
        _explodeState = new ExplodingEnemyExplodeState(this);

        _enemyStateMachine.AddTransition(_chaseState,_explodeState,() => Utility.CheckDistance(transform.position,Target.position,explodeDistance));
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
    private void OnDestroy() 
    {
        Destroy(_explodeTimerUI.gameObject);    
    }

    private void OnDrawGizmos() 
    {
        if(Target == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Ray ray = new Ray(frontCheck.position,frontCheck.forward);
        Gizmos.DrawRay(ray);   
    }
}