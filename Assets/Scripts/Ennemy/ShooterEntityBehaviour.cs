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
    [SerializeField] private float angularSpeed = 10;

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
            animator.SetBool("IsAttacking", true);
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
            animator.SetBool("IsAttacking", false);

        }

        /*
        animationSpeed = _rb.velocity.magnitude / ((_rb.angularVelocity.magnitude +1) * agent.speed);
        Debug.Log("anim speed = " + animationSpeed);
        Debug.Log("rb vel" + _rb.velocity.magnitude);
        Debug.Log("ag speed" + agent.speed);
        Debug.Log("angl speed" + _rb.angularVelocity.magnitude);

        animator.SetFloat("Speed", animationSpeed); */
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
        Vector3 direction = Vector3.Normalize(transform.position - FPSController.Instance.transform.position);

        Vector3 dest = FPSController.Instance.transform.position + _distanceToPlayer * direction;

        agent.SetDestination(dest);
        Debug.Log(dest);


        animationSpeed = (dest - transform.position).magnitude / (_rb.angularVelocity.magnitude +1);
        animator.SetFloat("Speed", animationSpeed);

        if (_betweenAttackTimer < 0.5) {
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