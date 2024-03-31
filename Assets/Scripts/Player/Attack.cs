using System.Collections;
using System.Collections.Generic;
using EzSoundManager;
using Player;
using UnityEngine;



public class Attack : MonoBehaviour
{
    private bool _inCombat;
    private bool _isLeftSideAttacking;
    private bool _isRightSideAttacking;

    private bool _canLeftAttack;
    private bool _canRightAttack;
    
    private Animator _characterAnimator;
    public float activeHitBoxDuration = 1.1f;
    public float startUpDuration = 0.7f;
    public float damage = 10;
    public GameObject hitBox;
    
    private readonly int LEFT = 0;
    private readonly int RIGHT = 1;

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
        hitBox.SetActive(false);
        
        _canLeftAttack = true;
        _canRightAttack = true;
    }

    void Update()
    {
        _currentSide = Side.None;
        
        if (Input.GetMouseButtonDown(0))
        {
            _currentSide = Side.Left;
            _currentAttack = HitType.LeftPunch;
            ProceedAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _currentSide = Side.Right;
            _currentAttack = HitType.RightPunch;
            ProceedAttack();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            _inCombat = !_inCombat;
            _characterAnimator.SetBool("inCombat", _inCombat);
        }
    }
    
    IEnumerator HitBoxCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        hitBox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hitBox.SetActive(false);
    }
    

    private void ProceedAttack()
    {
        if (_inCombat)
            StartCoroutine(HitBoxCoroutine());
        
        if (!CanAttack(_currentSide)) return;

        if (_currentSide == Side.Left)
        {
            _isLeftSideAttacking = true;
        }
        
        if (_currentSide == Side.Right)
        {
            _isRightSideAttacking = true;
        }
        
        _canLeftAttack = false;
        _canRightAttack = false;
        
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
        PlayPunchAnimation(_currentAttack);
        
    }
    
    private void StartCombo()
    {
        _currentComboIndex = 0;
        combos[_currentComboIndex].ResetCombo();
        ContinueCombo();
    }
    
    private void ContinueCombo()
    {
        PlayPunchAnimation(combos[_currentComboIndex].NextHit());
    }
    
    
    private void PlayPunchAnimation(HitType hitType)
    {
        _characterAnimator.SetTrigger(GetHitTypeHash(hitType));
    }
    
    private void AchieveComboReward()
    {
        StartCoroutine(PlayAnimationWithDelayInSeconds(0.4f));
    }
    
    
    IEnumerator PlayAnimationWithDelayInSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayPunchAnimation(HitType.QuadPunch);
        StartCoroutine(HitBoxCoroutine());
    }
    
    private bool CanAttack(Side side)
    {
        if (!_inCombat) return false;

        if (side == Side.Left) return _canLeftAttack;
        if (side == Side.Right) return _canRightAttack;

        return false;
    }
    
    private int GetHitTypeHash(HitType hitType)
    {
        return _hitTypeToHash[hitType.ToString()];
    }
    
    private bool IsPerformingCombo()
    {
        return _currentComboIndex != -1;
    }
    
    
    public void PunchStartEvent(int side)
    {
        SoundManager.PlaySoundOnGameObject("punch", "SFX/Player", gameObject, false);
        SoundManager.RaiseRandomSoundAmongCategory("SFX/Player/Voice/PunchVoice", gameObject, false);
    }

    public void PunchEvent(int side)
    {
        if (side == LEFT && !_isRightSideAttacking)
        {
            _canRightAttack = true;
        }

        if (side == RIGHT && !_isLeftSideAttacking)
        {
            _canLeftAttack = true;
        }
    }
    
    public void PunchEndEvent(int side)
    {
        if (IsPerformingCombo() && combos[_currentComboIndex].IsComboFinished())
        {
            Debug.Log("Combo Finished!");
            combos[_currentComboIndex].ResetCombo();
            _currentComboIndex = -1;
            AchieveComboReward();
        }
        
        if(side == LEFT)
        {
            _isLeftSideAttacking = false;
            _canLeftAttack = true;
        }

        if (side == RIGHT)
        {
            _isRightSideAttacking = false;
            _canRightAttack = true;
        }

    }
}
