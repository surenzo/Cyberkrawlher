using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ShooterEntityBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject bullet;
    private NavMeshAgent agent;
    private float animationSpeed;
    private Animator animator;

    [SerializeField] private int chargeur;
    private int currentChargeur;

    [SerializeField] private float shotFrequency;
    private float shotTimer;


    private void Start()
    {
        Type = entityType.shooter;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer<0 && currentChargeur < chargeur)
        {
            GameObject go = Instantiate(bullet, bullet.transform);
            go.transform.SetParent(transform.parent, true);
            go.SetActive(true);
            go.GetComponent<Bullet>().direction = FPSController.Instance.transform.position - transform.position;
            go.GetComponent<Bullet>().manager = _playerManager;

            Debug.Log("Bang");

            currentChargeur += 1;
            shotTimer = shotFrequency;
        }
        else if (currentChargeur >= chargeur)
        {
            agent.isStopped = false;
            //Debug.Log(agent.destination);

        }
    }

    protected override bool Attacks()
    {
        agent.isStopped = true;

        shotTimer = shotFrequency;
        currentChargeur = 0;

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