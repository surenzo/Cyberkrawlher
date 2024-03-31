using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LightCurrencySystem
{
    public class Lighter : MonoBehaviour
    {
        [SerializeField] private TMP_Text costDisplay;
        [SerializeField] private TMP_Text lightAmountDisplay;
        [SerializeField] private OwnedLights ownedLights;
        [SerializeField] private float rayLength;
        public bool isAimingAtLightable;
        private static Transform _camTransform;
        private CyberCrawlerInputActions _inputActions;
        private LightableObjects _target;
        private RaycastHit _raycastHit;

        private void Start()
        {
            _camTransform = Camera.main.transform;
        }

        private void Awake()
        {
            _inputActions = new CyberCrawlerInputActions();
            if (lightAmountDisplay.IsUnityNull()) return;
            lightAmountDisplay.text = $"Lights: {ownedLights.lightsInPossession}";
        }

        private void OnEnable()
        {
            _inputActions.Player.ToggleLight.Enable();
            _inputActions.Player.ToggleLight.performed += OnToggleLight;
        }

        private void OnDisable()
        {
            _inputActions.Player.ToggleLight.Disable();
            _inputActions.Player.ToggleLight.performed -= OnToggleLight;
        }

        private void OnToggleLight(InputAction.CallbackContext callbackContext)
        {
            //Debug.Log("Got there");
            if (isAimingAtLightable)
            {
                ActivateLight();
                lightAmountDisplay.text = $"Lights: {ownedLights.lightsInPossession}";
            }
        }

        private void FixedUpdate()
        {
            //Debug.DrawRay(_camTransform.position, _camTransform.forward * 1000, Color.yellow);
            if (Physics.Raycast(_camTransform.position, _camTransform.forward, out _raycastHit, rayLength))
            {
                if (_raycastHit.collider.gameObject.layer == 7 &&
                    (!isAimingAtLightable ||
                     _target != _raycastHit.collider.gameObject.GetComponent<LightableObjects>()))
                {
                    //Debug.Log("Found new target");
                    //Debug.Log(_raycastHit.collider.gameObject.GetComponent<LightableObjects>());
                    if (_target != null)
                    {
                        _target.UnHighLight(); //stops highlighting the previous one in case there was a change between 2 neons without a pause 
                    }

                    _target = _raycastHit.collider.gameObject.GetComponent<LightableObjects>();
                    _target.Highlight();
                    costDisplay.enabled = true;
                    isAimingAtLightable = true;
                    if (ownedLights.lightsInPossession >= _target.lightCost && !_target.isLitUp)
                    {
                        costDisplay.text = $"Cost: {_target.lightCost} \n{_inputActions.Player.ToggleLight.GetBindingDisplayString()} to light up";
                    }
                    else if(!_target.isLitUp)
                    {
                        costDisplay.text = $"Cost: {_target.lightCost} \nNot enough light";
                    }
                    else
                    {
                        _target.Highlight();
                        costDisplay.enabled = false;
                    }
                }
            }
            else
            {
                if (isAimingAtLightable)
                {
                    _target.UnHighLight();
                    isAimingAtLightable = false;
                }
                costDisplay.enabled = false;
            }
        }

        private void ActivateLight()
        {
            //Debug.Log("Activating light");
            if (_target == null || !isAimingAtLightable) return;
            if (_target.isLitUp) return;
            if (ownedLights.lightsInPossession >= _target.lightCost)
            {
                _target.LitUp();
                ownedLights.lightsInPossession -= _target.lightCost;
                costDisplay.enabled = false;
            }
        }
    }
}