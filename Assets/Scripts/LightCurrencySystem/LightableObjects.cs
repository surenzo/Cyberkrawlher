using UnityEngine;

namespace LightCurrencySystem
{
    public class LightableObjects : MonoBehaviour
    {
        [SerializeField] private Color disabledColor;
        [SerializeField] private Color highlightColor;
        [SerializeField] private Color enabledColor;
        [SerializeField] private SphereCollider litArea;
        private Material _material;
        public bool isLitUp;
        public int lightCost = 100;

        private void Start()
        {
            _material = GetComponent<MeshRenderer>().material;
            _material.color = isLitUp ? enabledColor : disabledColor;
        }

        public void LitUp()
        {
            _material.color = enabledColor;
            isLitUp = true;
            litArea.enabled = true;
            litArea.radius = lightCost / 20;
            litArea.isTrigger = true;
            litArea.center = transform.position;
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
