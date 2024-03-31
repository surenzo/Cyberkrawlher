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

    [SerializeField] private int chargeur;

    [SerializeField] private float punchDuration;
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
            animator.SetBool("isAttacking", false);
        }
    }

    protected override bool Attacks()
    {
        agent.isStopped = true;

        punchTimer = punchDuration;
        punchHitBox.SetActive(true);
        animator.SetBool("isAttacking", true);

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