using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEntityBehaviour : AbstractEntityBehaviour
{
    [SerializeField] private GameObject bullet;

    private void Start()
    {
        Type = entityType.shooter;
    }

    protected override bool Attacks()
    {
        GameObject go = Instantiate(bullet, bullet.transform);
        go.transform.SetParent(transform, true);
        go.SetActive(true);
        go.GetComponent<Bullet>().direction = FPSController.Instance.transform.position - transform.position;
        return true;
    }

    /* TODO : FAIRE UN NAVMESH POUR MOVE */
    protected override void Move()
    {
        Vector3 PlayerPos = FPSController.Instance.transform.position;
        _rb.velocity = PlayerPos - transform.position;
    }

}
