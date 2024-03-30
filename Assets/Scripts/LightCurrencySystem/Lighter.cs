using UnityEngine;

namespace LightCurrencySystem
{
    public class Lighter : MonoBehaviour
    {
        private static Camera _cam;
        [SerializeField] private float rayLength;
        public bool isAimingAtLightable;
        private static float _screenWidth = Screen.width / 2f;
        private static float _screenHeight = Screen.height / 2f;
        private static Vector3 _rayInfo = new(_screenWidth, _screenHeight, 0);
        private static Ray _ray = _cam.ScreenPointToRay(_rayInfo);
        private LightableObjects _target;
        private RaycastHit _raycastHit;
        [SerializeField] private OwnedLights ownedLights;

        private void FixedUpdate()
        {
            if (Physics.Raycast(_ray.origin, _ray.direction, out _raycastHit, rayLength, 7))
            {
                if (!isAimingAtLightable || _target != _raycastHit.collider.gameObject.GetComponent<LightableObjects>())
                {
                    _target.UnHighLight();//stops highlighting the previous one in case there was a change between 2 neons without a pause 
                    _target = _raycastHit.collider.gameObject.GetComponent<LightableObjects>();
                    _target.Highlight();
                }

                isAimingAtLightable = true;
            }
            else
            {
                if (isAimingAtLightable) _target.UnHighLight();
                isAimingAtLightable = false;
            }
        }

        private void ActivateLight()
        {
            if (_target == null) return;
            if (_target.isLitUp) return;
            if (ownedLights.lightsInPossession >= _target.lightCost)
            {
                _target.LitUp();
                ownedLights.lightsInPossession -= _target.lightCost;
            }
        }
    }
}