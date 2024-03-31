using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private static readonly int LeftPunch = Animator.StringToHash("LeftPunch");
    private static readonly int RightPunch = Animator.StringToHash("RightPunch");
    private bool _inCombat;
    private Animator _characterAnimator;
    public float activeHitBoxDuration = 1.1f;
    public float startUpDuration = 0.7f;
    
    [SerializeField] private GameObject _hitBox;
    
    private static readonly int InCombat = Animator.StringToHash("inCombat");

    void Start()
    {
        _characterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!_inCombat) return;
            StartCoroutine(AttackCoroutine());
        }

        if (Input.GetMouseButtonDown(1))
        {
            _inCombat = !_inCombat;
            _characterAnimator.SetBool("inCombat", _inCombat);
        }
    }
    
    IEnumerator AttackCoroutine()
    {
        
        yield return new WaitForSeconds(startUpDuration);
        _hitBox.SetActive(true);
        yield return new WaitForSeconds(activeHitBoxDuration);
        _hitBox.SetActive(false);
    }
}
