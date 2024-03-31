using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BoxeEntityBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject punchHitBox;
    
    private float punchTimer;
    private HealthSystem _healthSystem;

    private void Start()
    {
        Type = entityType.boxer;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _healthSystem = GetComponent<HealthSystem>();
        
        
    }
    private void LateUpdate()
    {
        punchTimer -= Time.deltaTime;

        if (punchTimer < 0)
        {
            agent.isStopped = false;
            punchHitBox.SetActive(false);
            animator.SetBool("IsAttacking", false);
        }
    }

    protected override bool Attacks()
    {
        if (Vector3.Distance(transform.position, FPSController.Instance.transform.position) > 3) return true;
        agent.isStopped = true;

        punchTimer = _attackDuration;
        punchHitBox.SetActive(true);
        animator.SetBool("IsAttacking", true);

        return true;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            var damage = other.gameObject.GetComponentInParent<Attack>().damage;
            _healthSystem.Damage(damage);
        }
    }

}