using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumItem : MonoBehaviour
{
    public int lumQuantity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == FPSController.Instance.gameObject)
        {
            /* TODO : ADD LUM QUANTITY TO PLAYER */
            Destroy(this);
        }
    }
}
