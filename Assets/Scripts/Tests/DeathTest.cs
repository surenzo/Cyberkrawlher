using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTest : MonoBehaviour
{
    private HealthSystem _healthSystem;
    
    void Start()
    {
        _healthSystem = GetComponent<HealthSystem>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _healthSystem.Damage(100);
        }
    }
}
