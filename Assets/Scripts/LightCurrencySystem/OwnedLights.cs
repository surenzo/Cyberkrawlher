using UnityEngine;

namespace LightCurrencySystem
{
    [CreateAssetMenu(fileName = "OwnedLights", menuName = "ScriptableObjects/OwnedLights", order = 1)]
    public class OwnedLights : ScriptableObject
    {
        [SerializeField] private int lightsAtLaunch = 300;
        public int lightsInPossession;

        public void OnEnable()
        {
            lightsInPossession = lightsAtLaunch;
        }
    }
}