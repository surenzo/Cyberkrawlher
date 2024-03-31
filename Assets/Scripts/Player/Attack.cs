using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;


public class Attack : MonoBehaviour
{
    private bool _inCombat;
    private bool _isAttacking;
    private bool _isLeftSideAttacking;
    private bool _isRightSideAttacking;
    private Animator _characterAnimator;
    public float activeHitBoxDuration = 1.1f;
    public float startUpDuration = 0.7f;
    public float damage = 10;

    private enum Side
    {
        Left,
        Right,
        None
    }
    
    private Side _currentSide = Side.None;

    private int _currentComboIndex = -1;
    
    private HitType _currentAttack;
    
    public Combo[] combos; 
    
    private Dictionary<string, int> _hitTypeToHash = new Dictionary<string, int>
    {
        {"LeftPunch", Animator.StringToHash("LeftPunch")},
        {"RightPunch", Animator.StringToHash("RightPunch")},
        {"QuadPunch", Animator.StringToHash("QuadPunch")}
        
    };
    
    [SerializeField] private GameObject _hitBox;
    
    private static readonly int InCombat = Animator.StringToHash("inCombat");

    void Start()
    {
        _characterAnimator = GetComponent<Animator>();
        _currentComboIndex = -1;
        combos[0].ResetCombo();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentSide = Side.Left;
            _currentAttack = HitType.LeftPunch;
            coucou();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _currentSide = Side.Right;
            _currentAttack = HitType.RightPunch;
            coucou();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _inCombat = !_inCombat;
            _characterAnimator.SetBool("inCombat", _inCombat);
        }
    }

    private void coucou()
    {
        if (!CanAttack()) return;

        if (IsPerformingCombo() && combos[_currentComboIndex].CheckNextHit(_currentAttack))
        {
            ContinueCombo();
            return;
        }
        
        if (combos[0].CheckNextHit(_currentAttack))
        {
            StartCombo();
            return;
        }
        
        combos[0].ResetCombo();
        _isAttacking = true;
        _characterAnimator.SetTrigger(GetHitTypeHash(_currentAttack));
        StartCoroutine(AttackCoroutine(0.2f));
        
    }
    
    private void StartCombo()
    {
        _currentComboIndex = 0;
        combos[_currentComboIndex].ResetCombo();
        ContinueCombo();
    }
    
    private void ContinueCombo()
    {
        _characterAnimator.SetTrigger(GetHitTypeHash(combos[_currentComboIndex].NextHit()));
        _isAttacking = true;
        StartCoroutine(AttackCoroutine(0.2f));
    }
    
    IEnumerator AttackCoroutine(float timeToWaitSecond)
    {
        float startTime = Time.time;
        
        startTime = Time.time;
        
        while (Time.time - startTime <= timeToWaitSecond)
        {
            yield return null;
        }
        
        _isAttacking = false;
        if (IsPerformingCombo() && combos[_currentComboIndex].IsComboFinished())
        {
            Debug.Log("Combo Finished!");
            combos[_currentComboIndex].ResetCombo();
            _currentComboIndex = -1;
            AchieveComboReward();
        }
    }
    
    private void AchieveComboReward()
    {
        StartCoroutine(PlayAnimationWithDelayInSeconds(0.4f));
    }
    
    
    IEnumerator PlayAnimationWithDelayInSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        _characterAnimator.SetTrigger(GetHitTypeHash(HitType.QuadPunch));
        _isAttacking = true;
        StartCoroutine(AttackCoroutine(0.9f));
    }
    
    
    
    /*IEnumerator AttackCoroutine()
    {
        _characterAnimator.SetTrigger(LeftPunch);
        yield return new WaitForSeconds(startUpDuration);
        _hitBox.SetActive(true);
        yield return new WaitForSeconds(activeHitBoxDuration);
        _hitBox.SetActive(false);
    }*/

    private bool CanAttack()
    {
        return _inCombat && !_isAttacking;
    }
    
    private int GetHitTypeHash(HitType hitType)
    {
        return _hitTypeToHash[hitType.ToString()];
    }
    
    private bool IsPerformingCombo()
    {
        return _currentComboIndex != -1;
    }
}
