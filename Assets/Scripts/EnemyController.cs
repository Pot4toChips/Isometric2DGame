using UnityEngine;
using UnityEngine.AI;

public class EnemyController : IsometricController
{
    [Header("Pathfinding")]
    [SerializeField] private Transform _target;
    [SerializeField] private NavMeshAgent _agent;

    [Header("Animation")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("State Machine")]
    [SerializeField] private float _detectionRange;
    [SerializeField] private float _fieldOfView;
    [SerializeField] private float _attackRange;

    private EnemyBaseState _currentState;
    private EnemyIdleState _idleState;
    private EnemyPatrolState _patrolState;
    private EnemyChaseState _chaseState;
    private EnemyAttackState _attackState;

    // Isometric Controller Variables
    public Rigidbody2D Rb => _rb;
    public float MoveSpeed => _moveSpeed;
    public Vector2 MoveDirection
    {
        get => _moveDirection;
        set => _moveDirection = value;
    }

    // Local Variables
    public Transform Target => _target;
    public NavMeshAgent Agent => _agent;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;
    public float DetectionRange => _detectionRange;
    public float FieldOfView => _fieldOfView;
    public float AttackRange => _attackRange;

    public EnemyIdleState IdleState => _idleState;
    public EnemyPatrolState PatrolState => _patrolState;
    public EnemyChaseState ChaseState => _chaseState;
    public EnemyAttackState AttackState => _attackState;

    private void Start()
    {
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _idleState = new EnemyIdleState(this);
        _patrolState = new EnemyPatrolState(this);
        _chaseState = new EnemyChaseState(this);
        _attackState = new EnemyAttackState(this);

        SwitchState(_idleState);
    }

    protected override void Update()
    {
        base.Update();
        _currentState.UpdateState();
    }

    public void Setup(Transform target)
    {
        _target = target;
    }

    public void SwitchState(EnemyBaseState _baseState)
    {
        _currentState = _baseState;
        _currentState.EnterState();
    }

    protected override void HandleInput()
    {
        _currentState.HandleInput();
    }

    protected override void HandleAnimation()
    {
        _currentState.HandleAnimation();
    }
}
