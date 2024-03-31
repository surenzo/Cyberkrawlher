using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ShooterEntityBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] public int chargeur;
    private int currentChargeur;

    [SerializeField] public float shotFrequency;
    private float shotTimer;
    
    private HealthSystem _healthSystem;
    

    private void Start()
    {
        Type = entityType.shooter;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        _healthSystem = GetComponent<HealthSystem>();
    }
    private void LateUpdate()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer<0 && currentChargeur < chargeur)
        {
            animator.SetBool("IsAttacking", true);
            GameObject go = Instantiate(bullet, bullet.transform);
            go.transform.SetParent(transform.parent, true);
            go.SetActive(true);
            go.GetComponent<Bullet>().direction = FPSController.Instance.transform.position - transform.position;
            go.GetComponent<Bullet>().manager = _playerManager;

            currentChargeur += 1;
            shotTimer = shotFrequency;

        }
        else if (currentChargeur >= chargeur)
        {
            agent.isStopped = false;
            animator.SetBool("IsAttacking", false);

        }
    }

    protected override bool Attacks()
    {
        agent.isStopped = true;

        shotTimer = shotFrequency;
        currentChargeur = 0;

        return true;
    }

}