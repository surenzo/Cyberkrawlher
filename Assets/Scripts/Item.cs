using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemEffects
    {
        //AttackBoost,
        //Defboost,
        //LightReceptionBoost,
        LightLightRegen,
        MedLightRegen,
        BigLightRegen,
        //LightEmissionBoost,
        SpeedBoost,
        SmallHealthRegen,
        MedHealthRegen,
        BigHealthRegen,
        StaminaBoost
    }

    public ItemEffects effect;
}
