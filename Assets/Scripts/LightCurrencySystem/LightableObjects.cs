using UnityEngine;

namespace LightCurrencySystem
{
    public class LightableObjects : MonoBehaviour
    {
        [SerializeField] private Color highlightColor;
        [SerializeField] private Color enabledColor;
        [SerializeField] private GameObject enabledNeon;
        [SerializeField] private GameObject neonLight;
        [SerializeField] private bool isVariant;

        private Material _material;
        public bool isLitUp;
        public int lightCost = 100;
        private Color _defaultColor;

        private void Start()
        {
            _material = GetComponent<MeshRenderer>().material;
            _defaultColor = _material.color;
            _material.color = isLitUp ? enabledColor : _defaultColor;
            if (isVariant)
            {
                enabledNeon.SetActive(false);
                neonLight.SetActive(false);
                neonLight.GetComponent<Light>().color = enabledColor;
            }
        }

        public void LitUp()
        {
            _material.color = enabledColor;
            isLitUp = true;
            
            if (isVariant)
            {
                GetComponent<MeshRenderer>().enabled = false;
                enabledNeon.SetActive(true);
                neonLight.SetActive(true);
            }
        }

        public void Highlight()
        {
            _material.color = isLitUp ? enabledColor : highlightColor;
        }

        public void UnHighLight()
        {
            _material.color = isLitUp ? enabledColor : _defaultColor;
        }
    }
}
