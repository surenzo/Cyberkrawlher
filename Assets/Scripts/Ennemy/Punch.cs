using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public PlayerManager manager;
    [SerializeField] private float damage;

    [SerializeField] private float duration;

    // Update is called once per frame
    void Update()
    {
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
