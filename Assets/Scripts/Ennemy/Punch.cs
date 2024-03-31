using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
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
        if(duration < 0) gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == manager.gameObject)
        {
            manager.healthSystem.Damage(damage);
        }
    }
}
