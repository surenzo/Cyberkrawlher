using System;
using LightCurrencySystem;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Image hpBar;
        [SerializeField] private Image staminaBar;
        [SerializeField] private float minBarLength = 0.27f;
        [SerializeField] private float maxBarLength = 0.75f;
        [SerializeField] private float lightMultiplicator = 0.3f;
        [SerializeField] private new Light light;
        [SerializeField] private float maxIntensity;
        [SerializeField] private float intensitySpeed;
        [SerializeField] public HealthSystem healthSystem;
        [SerializeField] private OwnedLights ownedLights;
        [SerializeField] private FPSController FPSController;
        [SerializeField] private float remainingHealthWarning = 0.2f;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color warningColor;
        [SerializeField] public int damage { get; private set; }
        private float _startTime;
        private float _lastHealth = 0;
        private float _barRange;

        private void Start()
        {
            _barRange = maxBarLength - minBarLength;
        }

        private void Update()
        {
            light.intensity = ownedLights.lightsInPossession*lightMultiplicator;
            if (light.intensity > maxIntensity) light.intensity = maxIntensity;
            if (healthSystem._health / healthSystem._maxLife < remainingHealthWarning)
            {
                light.color = warningColor;
            }
            else
            {
                light.color = normalColor;
            }

            if (healthSystem._health != _lastHealth)
            {
                _lastHealth = healthSystem._health;
                hpBar.fillAmount = healthSystem._health / healthSystem._maxLife * _barRange + minBarLength;
            }
            
            //TODO implement the same for stamina when it exists;
        }


    }
}
