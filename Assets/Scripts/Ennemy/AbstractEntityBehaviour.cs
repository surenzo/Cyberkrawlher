using Player;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthSystem))]

public abstract class AbstractEntityBehaviour : MonoBehaviour
{
    
    
    [SerializeField] public float _attackFrequency;
    [SerializeField] protected float _attackDuration;
    private float _attackTimer;

    [SerializeField] protected int _maxHealth;
    [SerializeField] private float _damageCooldown;
    private float _damageTimer;

    [SerializeField] private float _knockbackCoeff = 1;
    protected Rigidbody _rb;

    [SerializeField] protected int _lightLootQuantity;
    private float _currentHealth;

    [SerializeField] public int _speed;
    [SerializeField] public int _aggroRange = 40;
    [SerializeField] public float _distanceToPlayer;

    private GameObject _player;
    private GameObject _playerPunch;
    protected PlayerManager _playerManager;

    public NavMeshAgent agent;
    protected Animator animator;
    [SerializeField] private float angularSpeed = 10;
    
    private HealthSystem _healthSystem;


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
        

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(_damageTimer < 0)
        {
            _damageTimer = _damageCooldown;
            
            if(other.gameObject.layer == 9)
            {
                var damage = other.gameObject.GetComponentInParent<Attack>().damage;
                _healthSystem.Damage(damage);
            }
            
            if (_healthSystem._isDead)
            {
                EntityPool.Instance.MakeLum(transform.position);
                Debug.Log("lum cr  e");
                EntityPool.Instance.GoBack(gameObject);
                Debug.Log("entit  rang e");

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
        //_playerPunch = _player.GetComponent<Attack>().hitBox; 
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        _healthSystem = GetComponent<HealthSystem>();
        
        _healthSystem._maxLife = _maxHealth;
        _healthSystem._health = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector3.Distance(transform.position, FPSController.Instance.transform.position) > _aggroRange)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0f);
            return;
        }*/
        
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
        
        if (Vector3.Distance(transform.position, _player.transform.position) > 300)
        {
            if (Type == entityType.boxer) EntityPool.BoxerToSpawnWithBoss += 1;
            else EntityPool.ShooterToSpawnWithBoss += 1;
            EntityPool.Instance.GoBack(gameObject);
        }
        _currentHealth = _healthSystem._health;
    }
    

}
