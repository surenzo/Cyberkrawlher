using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private static readonly int Attacking = Animator.StringToHash("isAttacking");
    private Animator _characterAnimator;
    
    [SerializeField] private GameObject _hitBox;
    
    void Start()
    {
        _characterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AttackCoroutine());
        }
    }
    
    IEnumerator AttackCoroutine()
    {
        _characterAnimator.SetTrigger(Attacking);
        yield return new WaitForSeconds(0.7f);
        _hitBox.SetActive(true);
        yield return new WaitForSeconds(1.1f);
        _hitBox.SetActive(false);
    }
}
