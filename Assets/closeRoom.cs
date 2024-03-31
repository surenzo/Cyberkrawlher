using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeRoom : MonoBehaviour
{
    [SerializeField] private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            door.SetActive(true);
        }
    }
}
