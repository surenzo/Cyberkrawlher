using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    [SerializeField] private float speed;
    public PlayerManager manager;
    [SerializeField] private float damage;

    [SerializeField] private float duration;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.Normalize(direction) * speed * Time.deltaTime;
        duration -= Time.deltaTime;
        if(duration < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == manager.gameObject)
        {
            manager.healthSystem.Damage(damage);
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject == manager.gameObject)
        {
            manager.healthSystem.Damage(damage);
        }
        Destroy(gameObject);
    }
}
