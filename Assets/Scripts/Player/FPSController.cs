using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using LightCurrencySystem;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    #region Variables

    public static FPSController Instance;
    public float boostDuration = 10f;
    public float smallRegenAmount = 5f;
    public float medRegenAmount = 10f;
    public float bigRegenAmount = 15f;
    public float attackSpeedBoost = 0.2f;
    public float jumpSpeedBoost = 5f;
    public float attackBoost;
    public float defBoost;
    public float lightEmissionBoost;
    public float lightObtentionBoost;
    public int lightLightRegen = 10;
    public int medLightRegen = 20;
    public int bigLightRegen = 30;
    public float walkSpeedBoost = 4f;
    public float runSpeedBoost = 3f;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float attackVal;
    public float defVal;
    public float lightEmission;
    public float lightObtention;
    public float staminaLossFactor;
    public float staminaGainFactor;
    public float staminaRegenBoost;
    public float staminaLossBoost;
    private bool _emptyStamina;
    
    private float timeUntilStaminaRegen = 0.8f;
    private float timeUntilStaminaRegenCounter = 0;
    
    
    private float movementDirectionY;
    private bool isRunning = false;
    private bool staminaCoroutineOnGoing = false;


    float curSpeedX = 0;
    float curSpeedY = 0;


    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    private Animator characterAnimator;
    private HealthSystem _healthSystem;
    [SerializeField] private OwnedLights ownedLights;
    [SerializeField] private Attack attack;
    [SerializeField] private TMP_Text lightAmountDisplay;

    [SerializeField] private CinemachineVirtualCamera DeathVirtualCamera;
    [HideInInspector] public bool canMove = true;
    private static readonly int IsDead = Animator.StringToHash("isDead");
    private static readonly int Speed = Animator.StringToHash("Speed");

    #endregion

    #region Initialisation

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Get Components
        characterController = GetComponent<CharacterController>();
        _healthSystem = GetComponent<HealthSystem>();
        characterAnimator = GetComponent<Animator>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #endregion

    #region Updates

    void Update()
    {
        PlayerMovement();
        DeathCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Item collectedItem = other.GetComponent<Item>();
            switch (collectedItem.effect)
            {
                case Item.ItemEffects.SmallHealthRegen:
                    _healthSystem.Heal(smallRegenAmount);
                    break;
                case Item.ItemEffects.MedHealthRegen:
                    _healthSystem.Heal(medRegenAmount);
                    break;
                case Item.ItemEffects.BigHealthRegen:
                    _healthSystem.Heal(bigRegenAmount);
                    break;
                case Item.ItemEffects.LightLightRegen:
                    ownedLights.lightsInPossession += lightLightRegen;
                    lightAmountDisplay.text = $"Lights: {ownedLights.lightsInPossession}";
                    break;
                case Item.ItemEffects.MedLightRegen:
                    ownedLights.lightsInPossession += medLightRegen;
                    lightAmountDisplay.text = $"Lights: {ownedLights.lightsInPossession}";
                    break;
                case Item.ItemEffects.BigLightRegen:
                    ownedLights.lightsInPossession += bigLightRegen;
                    lightAmountDisplay.text = $"Lights: {ownedLights.lightsInPossession}";
                    break;
                case Item.ItemEffects.SpeedBoost:
                    StartCoroutine(SpeedBoost());
                    break;
                case Item.ItemEffects.Defboost:
                    StartCoroutine(DefBoost());
                    break;
                case Item.ItemEffects.AttackBoost:
                    StartCoroutine(AtkBoost());
                    break;
                case Item.ItemEffects.LightEmissionBoost:
                    StartCoroutine(LightEmissionBoost());
                    break;
                case Item.ItemEffects.LightReceptionBoost:
                    StartCoroutine(LightObtentionBoost());
                    break;
                case Item.ItemEffects.StaminaBoost:
                    StartCoroutine(StaminaBoost());
                    break;
            }
            GameObject.Destroy(other.gameObject);
        }
    }

    #endregion

    #region Movement and Rotation

    private void PlayerMovement()
    {
        MoveCalcul();
        JumpAndGravity();
        PlayerRotation();
        Move();
    }

    private void MoveCalcul()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);
        curSpeedX = canMove ? ((isRunning && !_emptyStamina) ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        curSpeedY = canMove ? ((isRunning && !_emptyStamina) ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        bool condition = isRunning && !_emptyStamina && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                                                         Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow) ||
                                                         Input.GetKey(KeyCode.W)         || Input.GetKey(KeyCode.S) || 
                                                         Input.GetKey(KeyCode.A)         || Input.GetKey(KeyCode.D));
        if ( condition )
        {
            _healthSystem.stamina -= staminaLossFactor * Time.deltaTime;
            if (_healthSystem.stamina < 0)
            {
                _healthSystem.stamina = 0;
                _emptyStamina = true;
            }
            timeUntilStaminaRegenCounter = 0;
        }
        else
        {
            timeUntilStaminaRegenCounter += Time.deltaTime;
            if (timeUntilStaminaRegenCounter > timeUntilStaminaRegen)
            {
                _healthSystem.stamina += staminaGainFactor * Time.deltaTime;
                if (_healthSystem.stamina > _healthSystem.staminaThreshold)
                {
                    _emptyStamina = false;
                    if (_healthSystem.stamina > _healthSystem.maxStamina)
                    {
                        _healthSystem.stamina = _healthSystem.maxStamina;
                    }
                }
            }
            
            
            
        }
    }

    IEnumerator StaminaModifying()
    {
        
        
        yield return new WaitForSeconds(0.5f);
        while (!isRunning && _emptyStamina)
        {
            _healthSystem.stamina += staminaGainFactor * Time.deltaTime;
            if (_healthSystem.stamina > _healthSystem.staminaThreshold)
            {
                _emptyStamina = false;
                if (_healthSystem.stamina > _healthSystem.maxStamina)
                {
                    _healthSystem.stamina = _healthSystem.maxStamina;
                }
            }
        }
        staminaCoroutineOnGoing = false;
        yield return null;
    }
    
    

    private void JumpAndGravity()
    {
        // Jump
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        // Move the controller
        characterController.Move(moveDirection.normalized * (Time.deltaTime * (isRunning ? runningSpeed : walkingSpeed)));

        // Animation
        if (curSpeedX != 0 || curSpeedY != 0)
        {
            float speed = Math.Abs((moveDirection.x + moveDirection.z) / runningSpeed);
            characterAnimator.SetFloat(Speed, speed);
        }
        else
        {
            characterAnimator.SetFloat(Speed, 0);
        }
    }

    private void PlayerRotation()
    {
        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    #endregion

    #region Death

    private void DeathCheck()
    {
        if (_healthSystem._isDead)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        characterAnimator.SetTrigger(IsDead);
        canMove = false;
        _healthSystem._isDead = false;
        DeathVirtualCamera.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
    }

    #endregion

    #region LimitedTimeBoosts

    IEnumerator SpeedBoost()
    {
        jumpSpeed += runSpeedBoost;
        runningSpeed += jumpSpeedBoost;
        walkingSpeed += walkSpeedBoost;
        if (attack.startUpDuration > attackSpeedBoost && attack.activeHitBoxDuration > attackSpeedBoost)
        {
            attack.startUpDuration -= attackSpeedBoost;
            attack.activeHitBoxDuration -= attackSpeedBoost;
            yield return new WaitForSeconds(boostDuration);
            attack.startUpDuration += attackSpeedBoost;
            attack.activeHitBoxDuration += attackSpeedBoost;
        }

        walkingSpeed -= walkSpeedBoost;
        jumpSpeed -= runSpeedBoost;
        runningSpeed -= runSpeedBoost;
    }

    IEnumerator DefBoost()
    {
        defVal += defBoost;
        yield return new WaitForSeconds(boostDuration);
        defVal -= defBoost;
    }

    IEnumerator AtkBoost()
    {
        attackVal += attackBoost;
        yield return new WaitForSeconds(boostDuration);
        attackVal -= attackBoost;
    }

    IEnumerator LightEmissionBoost()
    {
        lightEmission += lightEmissionBoost;
        yield return new WaitForSeconds(boostDuration);
        lightEmission -= lightEmissionBoost;
    }

    IEnumerator LightObtentionBoost()
    {
        lightObtention += lightEmissionBoost;
        yield return new WaitForSeconds(boostDuration);
        lightObtention -= lightEmissionBoost;
    }

    IEnumerator StaminaBoost()
    {
        staminaGainFactor += staminaRegenBoost;
        if (staminaLossFactor > staminaLossBoost)
        {
            staminaLossFactor -= staminaLossBoost;
            yield return new WaitForSeconds(boostDuration);
        }

        staminaLossFactor += staminaLossBoost;
        staminaGainFactor -= staminaRegenBoost;
    }

    #endregion
}