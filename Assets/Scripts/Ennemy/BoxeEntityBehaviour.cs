using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxeEntityBehaviour : AbstractEntityBehaviour
{
    private void Start()
    {
        Type = entityType.boxer;
    }
    protected override bool Attacks()
    {
        return true;
    }

    /* TODO : FAIRE UN NAVMESH POUR MOVE */
    protected override void Move()
    {
        Vector3 PlayerPos = FPSController.Instance.transform.position;
        _rb.velocity = PlayerPos - transform.position;
        
    }

}
