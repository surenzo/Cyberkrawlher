using Player;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractBossBehaviour : MonoBehaviour
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
    [SerializeField] public int _shootingRange = 40;
    [SerializeField] public int _boxingRange = 40;
   // [SerializeField] protected float _distanceToPlayer;

    private GameObject _player;
    private GameObject _playerPunch;
    protected PlayerManager _playerManager;

    protected NavMeshAgent agent;
    protected Animator animator;
    [SerializeField] private float angularSpeed = 10;


    protected entityType Type;

    public bool isAttackFinished = false;
    protected float _betweenAttackTimer;


    public bool isRunning;


    public enum entityType
    {
        shooter,
        boxer,
        other
    }


    /* Returns false when the action is finished */
    protected abstract bool Attacks();

    protected void Move(Vector3 target, float distance)
    {

        Vector3 direction = Vector3.Normalize(transform.position - target);

        Vector3 dest = target + distance * direction;

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
        if (_damageTimer < 0)
        {
            _damageTimer = _damageCooldown;
            _rb.velocity += knockback * _knockbackCoeff * new Vector3(transform.position.x - DamageSource.position.x, 0, transform.position.y - DamageSource.position.y);
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                EntityPool.Instance.MakeLum(transform.position);
                EntityPool.Instance.GoBack(gameObject);
                _playerManager.healthSystem.Heal(5000);
            }
        }

    }



    // Start is called before the first frame update
    void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _currentHealth = _maxHealth;
        _player = FPSController.Instance.gameObject;
        _playerManager = _player.GetComponent<PlayerManager>();
        //_playerPunch = _player.GetComponent<Attack>().hitBox; 
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 target = FPSController.Instance.transform.position;
        float distance = 0;

        if (isAttackFinished)
        {
            int r = Random.Range(0, 3);
            Debug.Log(r);
            if (r == 0)
            {
                Type = entityType.shooter;
                distance = _shootingRange;
            }
            else if (r == 1)
            {
                Type = entityType.other;
                distance = 0.5f * (_shootingRange + _boxingRange);
            }
            else
            {
                Type = entityType.boxer;
                distance = _boxingRange;
            }
            isAttackFinished = false;
        }
        
        if(Mathf.Abs(Vector3.Distance(transform.position, target) - distance) > 0.5)
        {
            Move(target, distance);
        }
        else
        {
            Attacks();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _playerPunch)
        {
            Damage(_playerManager.damage, _player.transform, 1);
        }
    }

}
