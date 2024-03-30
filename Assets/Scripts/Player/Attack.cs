using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private static readonly int Attacking = Animator.StringToHash("isAttacking");
    private Animator _characterAnimator;
    public float activeHitBoxDuration = 1.1f;
    public float startUpDuration = 0.7f;
    
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
        yield return new WaitForSeconds(startUpDuration);
        _hitBox.SetActive(true);
        yield return new WaitForSeconds(activeHitBoxDuration);
        _hitBox.SetActive(false);
    }
}
