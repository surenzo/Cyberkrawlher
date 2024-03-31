using Player;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractEntityBehaviour : MonoBehaviour
{
    [SerializeField] protected float _attackFrequency;
    [SerializeField] protected float _attackDuration;
    private float _attackTimer;

    [SerializeField] protected int _maxHealth;
    [SerializeField] private float _damageCooldown;
    private float _damageTimer;

    [SerializeField] private float _knockbackCoeff = 1;
    protected Rigidbody _rb;

    [SerializeField] protected int _lightLootQuantity;
    private int _currentHealth;

    [SerializeField] protected int _speed;
    [SerializeField] public int _aggroRange = 40;
    [SerializeField] protected float _distanceToPlayer;

    private GameObject _player;
    private GameObject _playerPunch;
    protected PlayerManager _playerManager;

    protected NavMeshAgent agent;
    protected Animator animator;
    [SerializeField] private float angularSpeed = 10;


    public entityType Type { get; protected set; }

    public bool isAttacking = false;
    protected float _betweenAttackTimer;


    public bool isRunning;


    public enum entityType {
        shooter,
        boxer,
        other
    }


    /* Returns false when the action is finished */
    protected abstract bool Attacks();

    protected void Move()
    {

        Vector3 direction = Vector3.Normalize(transform.position - FPSController.Instance.transform.position);

        Vector3 dest = FPSController.Instance.transform.position + _distanceToPlayer * direction;

        agent.SetDestination(dest);


        float animationSpeed = (dest - transform.position).magnitude / (_rb.angularVelocity.magnitude + 1);
        animator.SetFloat("Speed", animationSpeed);

        if (_betweenAttackTimer < 0.5)
        {
            Vector3 targetDirection = FPSController.Instance.transform.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = angularSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public void Heal(int n)
    {
        if (n <= 0) return;
        _currentHealth += n;
    }

    public void Damage(int damage, Transform DamageSource, float knockback)
    {
        if(_damageTimer < 0)
        {
            _damageTimer = _damageCooldown;
            _rb.velocity += knockback * _knockbackCoeff * new Vector3(transform.position.x - DamageSource.position.x, 0, transform.position.y - DamageSource.position.y);
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                EntityPool.Instance.MakeLum(transform.position);
                Debug.Log("lum cr��e");
                EntityPool.Instance.GoBack(gameObject);
                Debug.Log("entit� rang�e");
            }
        }
    }



    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _currentHealth = _maxHealth;
        _player = FPSController.Instance.gameObject;
        _playerManager = _player.GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, FPSController.Instance.transform.position) > _aggroRange)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);
            return;
        }


        if (Vector3.Distance(transform.position, _player.transform.position) > 300)
        {
            if (Type == entityType.boxer) EntityPool.BoxerToSpawnWithBoss += 1;
            else EntityPool.ShooterToSpawnWithBoss += 1;
            EntityPool.Instance.GoBack(gameObject);
        }

        if (_betweenAttackTimer < 0)
        {
            Attacks();
            _betweenAttackTimer = _attackFrequency + _attackDuration;
            _attackTimer = _attackDuration;
            isAttacking = true;
        }
        else 
        {
            Move();
        }
        if (_attackTimer < 0)
        {
            isAttacking = false;
        }
        if(_damageTimer > -1) _damageTimer -= Time.deltaTime;
        if (_betweenAttackTimer > -1) _betweenAttackTimer -= Time.deltaTime;
        if (_attackTimer > -1) _attackTimer -= Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == _playerPunch)
        {
            Damage(_playerManager.damage, _player.transform, 1);
        }
    }

}
