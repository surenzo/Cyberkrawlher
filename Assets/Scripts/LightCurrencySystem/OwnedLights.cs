using UnityEngine;

namespace LightCurrencySystem
{
    [CreateAssetMenu(fileName = "OwnedLights", menuName = "ScriptableObjects/OwnedLights", order = 1)]
    public class OwnedLights : ScriptableObject
    {
        public int lightsInPossession;
    }
}