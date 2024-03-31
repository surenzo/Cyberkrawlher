using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BossBehaviour : AbstractBossBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private int chargeur;
    private int currentChargeur;

    [SerializeField] private float shotFrequency;
    private float shotTimer;

    [SerializeField] private GameObject punchHitBox;

    private float punchTimer;

    [SerializeField] private float shockWaveDuration;
    [SerializeField] private float shockWaveSpeed;
    private float shockWaveTimer;
    [SerializeField] private GameObject shockwave;
    [SerializeField] private Vector3 shockScale;




    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        if (Type == entityType.shooter)
        { 
            ShooterUpdate();
            return;
        }
        if (Type == entityType.boxer)
        {
            BoxerUpdate();
            return;
        }
        if (Type == entityType.other)
        {
            OtherUpdate();
            return;
        }

    }



    private void ShooterUpdate()
    {
        shotTimer -= Time.deltaTime;

        if (shotTimer < 0 && currentChargeur < chargeur)
        {
            animator.SetBool("IsShooting", true);
            GameObject go = Instantiate(bullet, bullet.transform);
            go.transform.SetParent(transform.parent, true);
            go.SetActive(true);
            go.GetComponent<Bullet>().direction = FPSController.Instance.transform.position - bullet.transform.position;

            Debug.Log("-----------");
            Debug.Log(FPSController.Instance.transform.position);
            Debug.Log(go.transform.position);
            Debug.Log(transform.position);
            go.GetComponent<Bullet>().manager = _playerManager;

            currentChargeur += 1;
            shotTimer = shotFrequency;

        }
        else if (currentChargeur >= chargeur)
        {
            agent.isStopped = false;
            animator.SetBool("IsShooting", false);
            isAttackFinished = false;

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
            isAttackFinished = false;
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



    private void OtherUpdate()
    {
        shockWaveTimer -= Time.deltaTime;
        shockwave.transform.localScale = shockWaveSpeed * (shockWaveDuration - shockWaveTimer) * shockScale;
        if (shockWaveTimer < 0)
        {
            agent.isStopped = false;
            shockwave.SetActive(false);
            animator.SetBool("Shockwave", false);
            isAttackFinished = false;
        }
    }

    private bool OtherAttack()
    {
        agent.isStopped = true;

        shockWaveTimer = shockWaveDuration;
        shockwave.SetActive(true);
        shockwave.transform.localScale = shockScale;
        animator.SetBool("Shockwave", true);

        return true;
    }




    protected override bool Attacks()
    {
        switch (Type)
        {
            case entityType.shooter: ShooterAttack(); break;
            case entityType.boxer: BoxerAttack(); break;
            case entityType.other: OtherAttack(); break;
        }

        return true;
    }

}