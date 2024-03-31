using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [System.Serializable]
    public struct Combo
    {
        public HitType[] combo;
        
        
        private int _currentHitIndex;

        public HitType NextHit()
        {
            return combo[++_currentHitIndex];
        }
        
        public bool IsComboFinished()
        {
            return _currentHitIndex >= combo.Length - 1;
        }
        
        public void ResetCombo()
        {
            _currentHitIndex = -1;
        }
        
        public bool CheckNextHit(HitType hitType)
        {
            if(_currentHitIndex >= combo.Length - 1)
                return false;
            return combo[_currentHitIndex + 1].CompareTo(hitType) == 0;
        }
    }
}
