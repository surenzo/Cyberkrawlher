using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterEntityBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject bullet;
    private NavMeshAgent agent;
    private float animationSpeed;
    private Animator animator;


    private void Start()
    {
        Type = entityType.shooter;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected override bool Attacks()
    {
        GameObject go = Instantiate(bullet, bullet.transform);
        go.transform.SetParent(transform.parent, true);
        go.SetActive(true);
        go.GetComponent<Bullet>().direction = FPSController.Instance.transform.position - transform.position;
        go.GetComponent<Bullet>().manager = _playerManager;

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