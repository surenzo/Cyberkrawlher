using UnityEngine;

namespace LightCurrencySystem
{
    public class Lighter : MonoBehaviour
    {
        private static Transform _camTransform;
        [SerializeField] private float rayLength;
        public bool isAimingAtLightable;
        private LightableObjects _target;
        private RaycastHit _raycastHit;
        [SerializeField] private OwnedLights ownedLights;

        private void Start()
        {
            _camTransform = Camera.main.transform;
        }

        private void FixedUpdate()
        {
            //Debug.DrawRay(_camTransform.position, _camTransform.forward * 1000, Color.yellow);
            if (Physics.Raycast(_camTransform.position, _camTransform.forward, out _raycastHit, rayLength))
            {
                if (_raycastHit.collider.gameObject.layer == 7 &&
                    (!isAimingAtLightable || _target != _raycastHit.collider.gameObject.GetComponent<LightableObjects>()))
                {
                    //Debug.Log("Found new target");
                    //Debug.Log(_raycastHit.collider.gameObject.GetComponent<LightableObjects>());
                    if (_target != null)
                        _target.UnHighLight(); //stops highlighting the previous one in case there was a change between 2 neons without a pause 
                    _target = _raycastHit.collider.gameObject.GetComponent<LightableObjects>();
                    _target.Highlight();
                    isAimingAtLightable = true;
                }
            }
            else
            {
                if (isAimingAtLightable) _target.UnHighLight();
                isAimingAtLightable = false;
            }

            if (isAimingAtLightable) //TODO change to implement button press
            {
                ActivateLight();
            }
        }

        private void ActivateLight()
        {
            //Debug.Log("Activating light");
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