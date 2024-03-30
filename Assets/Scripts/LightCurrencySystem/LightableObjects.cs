using UnityEngine;

namespace LightCurrencySystem
{
    public class LightableObjects : MonoBehaviour
    {
        public int lightCost = 100;
        [SerializeField] private Color disabledColor;
        [SerializeField] private Color highlightColor;
        [SerializeField] private Color enabledColor;
        private Material _material;
        public bool isLitUp;

        private void Start()
        {
            _material = GetComponent<Material>();
        }

        public void LitUp()
        {
            _material.color = enabledColor;
        }

        public void Highlight()
        {
            _material.color = isLitUp ? enabledColor : highlightColor;
        }

        public void UnHighLight()
        {
            _material.color = isLitUp ? enabledColor : disabledColor;
        }
    }
}
