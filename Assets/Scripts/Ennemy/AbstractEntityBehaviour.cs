using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] protected float _distanceToPlayer;

    private GameObject _player;
    private GameObject _playerPunch;
    protected PlayerManager _playerManager;


    public entityType Type { get; protected set; }

    public bool isAttacking = false;
    private float _betweenAttackTimer;



    public bool isRunning;


    public enum entityType {
        shooter,
        boxer
    }


    /* Returns false when the action is finished */
    protected abstract bool Attacks();

    protected abstract void Move();

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
                Debug.Log("lum créée");
                EntityPool.Instance.GoBack(gameObject);
                Debug.Log("entité rangée");

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
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_betweenAttackTimer);
        if (Vector3.Distance(transform.position, _player.transform.position) > 300) Destroy(gameObject);

        if (_betweenAttackTimer < 0)
        {
            Attacks();
            _betweenAttackTimer = _attackFrequency + _attackDuration;
            _attackTimer = _attackDuration;
            isAttacking = true;
        }
        else 
        {
            Debug.Log("sh'movin");
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
