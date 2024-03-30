using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEntityBehaviour : MonoBehaviour
{
    [SerializeField] protected float _attackFrequency;
    [SerializeField] private float _attackDuration;
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

    private void Damage(int damage, Transform DamageSource, float knockback)
    {
        _damageTimer = _damageCooldown;
        _rb.velocity += knockback * _knockbackCoeff * new Vector3(transform.position.x - DamageSource.position.x, 0, transform.position.y - DamageSource.position.y);

        if (_currentHealth <= 0)
        {
            EntityPool.Instance.MakeLum(_lightLootQuantity, transform);
            EntityPool.Instance.GoBack(this);
        }
    }



    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(_betweenAttackTimer < 0)
        {
            Attacks();
            _betweenAttackTimer = _attackFrequency + _attackDuration;
            _attackTimer = _attackDuration;
            isAttacking = true;
        }
        else if (Vector3.Distance(transform.position, FPSController.Instance.transform.position) > _distanceToPlayer)
        {
            Move();
        }
        if (_attackTimer < 0)
        {
            isAttacking = false;
        }
        _damageTimer -= Time.deltaTime;
        _betweenAttackTimer -= Time.deltaTime;
        _attackTimer -= Time.deltaTime;
    }
}
