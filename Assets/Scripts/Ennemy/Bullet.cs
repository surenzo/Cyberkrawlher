using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.Normalize(direction) * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == FPSController.Instance)
        {
            /* Damage player */
        }
        Debug.Log(other.gameObject.name);
        Destroy(gameObject);
    }
}
