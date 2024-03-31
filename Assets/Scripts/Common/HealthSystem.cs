using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float _maxLife;
    public float _health;
    public float maxStamina;
    public float stamina;
    public float staminaThreshold;
    
    internal bool _isDead = false;

    void Start()
    {
        _health = _maxLife;
        stamina = maxStamina;
    }


    public void Damage(float health)
    {
        _health -= health;
        Debug.Log("aie");
        if (_health <= 0)
        {
            _health = 0;
            _isDead = true;
            Debug.Log("Dead");
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
    
    public void Running(float stamina)
    {
        this.stamina -= stamina;
        if (this.stamina <= 0)
        {
            this.stamina = 0;
        }
    }
    
    public void Recovering(float stamina)
    {
        this.stamina += stamina;
        if (this.stamina > maxStamina)
        {
            this.stamina = maxStamina;
        }
    }
    
    
}