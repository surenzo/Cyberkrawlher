using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float _maxLife;
    public float _health;
    
    internal bool _isDead = false;

    void Start()
    {
        _health = _maxLife;
    }

    public void Damage(float health)
    {
        _health -= health;
        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
        }
    }

    public void Heal(float health)
    {
        _health += health;
        if (_health > _maxLife)
        {
            _health = _maxLife;
        }
    }

    public float GetHealth()
    { return _health; }

    public void SetMaxLife(float maxLife)
    {
        _maxLife = maxLife;
    }
}