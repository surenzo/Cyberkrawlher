using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BoxeEntityBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject punchHitBox;
    private NavMeshAgent agent;
    private float animationSpeed;
    private Animator animator;

    private float punchTimer;


    private void Start()
    {
        Type = entityType.shooter;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
        if (Vector3.Distance(transform.position, FPSController.Instance.transform.position) > 1) return true;
        Debug.Log("kachow");
        agent.isStopped = true;

        punchTimer = _attackDuration;
        punchHitBox.SetActive(true);
        animator.SetBool("IsAttacking", true);

        return true;
    }



    protected override void Move()
    {
        agent.SetDestination(FPSController.Instance.transform.position);
        agent.stoppingDistance = _distanceToPlayer;

        animationSpeed = _rb.velocity.magnitude / (_rb.angularVelocity.magnitude * agent.speed);
        animator.SetFloat("Speed", animationSpeed);
    }
}