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

}