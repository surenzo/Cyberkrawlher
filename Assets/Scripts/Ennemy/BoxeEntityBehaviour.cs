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
    [SerializeField] private float angularSpeed = 10;


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



    protected override void Move()
    {
        Vector3 direction = Vector3.Normalize(transform.position - FPSController.Instance.transform.position);

        Vector3 dest = FPSController.Instance.transform.position + _distanceToPlayer * direction;

        agent.SetDestination(dest);
        Debug.Log(dest);

        animationSpeed = (dest - transform.position).magnitude / (_rb.angularVelocity.magnitude + 1);
        animator.SetFloat("Speed", animationSpeed);

        if (_betweenAttackTimer < 0.5)
        {
            Vector3 targetDirection = FPSController.Instance.transform.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = angularSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }
}