using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BossBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private int chargeur;
    private int currentChargeur;

    [SerializeField] private float shotFrequency;
    private float shotTimer;

    [SerializeField] private GameObject punchHitBox;

    private float punchTimer;


    private AbstractEntityBehaviour.entityType entityType;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        if (entityType == AbstractEntityBehaviour.entityType.shooter)
        { 
            ShooterUpdate();
            return;
        }
        if (entityType == AbstractEntityBehaviour.entityType.boxer)
        {
            BoxerUpdate();
            return;
        }

    }



    private void ShooterUpdate()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer < 0 && currentChargeur < chargeur)
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


    private bool ShooterAttack()
    {
        agent.isStopped = true;

        shotTimer = shotFrequency;
        currentChargeur = 0;

        return true;
    }


    private void BoxerUpdate()
    {
        punchTimer -= Time.deltaTime;

        if (punchTimer < 0)
        {
            agent.isStopped = false;
            punchHitBox.SetActive(false);
            animator.SetBool("IsAttacking", false);
        }
    }

    private bool BoxerAttack()
    {
        if (Vector3.Distance(transform.position, FPSController.Instance.transform.position) > 3) return true;
        agent.isStopped = true;

        punchTimer = _attackDuration;
        punchHitBox.SetActive(true);
        animator.SetBool("IsAttacking", true);

        return true;
    }





    protected override bool Attacks()
    {
        int r = Random.Range(0, 3);
        if (r == 0) return BoxerAttack();
        if (r == 1) return ShooterAttack();
        //if (r == 2) return OtherAttack();


        return true;
    }

}