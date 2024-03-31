using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BoxeEntityBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject punchHitBox;

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
        if (Vector3.Distance(transform.position, FPSController.Instance.transform.position) > 3) return true;
        Debug.Log("kachow");
        agent.isStopped = true;

        punchTimer = _attackDuration;
        punchHitBox.SetActive(true);
        animator.SetBool("IsAttacking", true);

        return true;
    }

}