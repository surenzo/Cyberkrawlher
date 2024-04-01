using System;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [Header("Hologram")] [SerializeField] private Mesh hologramMesh;
    [SerializeField] private List<GameObject> itemMeshes;

    [ColorUsage(true, true)] [SerializeField]
    private Color hologramColor;

    [ColorUsage(true, true)] [SerializeField]
    private Color hologramOutlineColor;


    [Header("Hologram transform settings")] [SerializeField]
    private Vector3 hologramSize = new Vector3(1, 1, 1);

    [SerializeField] private Vector3 hologramRotation = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 hologramPosition = new Vector3(0, 0, 0);

    [Header("Hologram movement settings")] [SerializeField]
    private float hologramSpeedRotation = 50f;

    [SerializeField] private float hologramAmplitudeMovement = 0.2f;
    [SerializeField] private float hologramSpeedMovement = 2f;

    [Header("Chest Settings")] [SerializeField]
    public bool isOpen = false;

    [Header("References")] [SerializeField]
    private GameObject hologram;

    [SerializeField] private GameObject rayHologram;
    [SerializeField] public EasterEggItem Egg;

    public int eggAmount = 2;
    private GameObject hologramMeshObject;
    private Vector3 holoScale;

    private Mesh _itemMesh;
    private Array _itemTypes;
    private int _itemTypeAmount;
    private int _selectedItem;
    private float initialY;
    private static readonly int FresnelColor = Shader.PropertyToID("FresnelColor");
    private static readonly int MainColor = Shader.PropertyToID("MainColor");
    public bool isLooted;
    public int amountChecked;
    private int _prevAmountChecked;
    private bool _reValidated;

    private void OnValidate()
    {
        hologram.SetActive(isOpen);
        rayHologram.SetActive(isOpen);
        hologram.transform.localScale = holoScale;
        hologramMeshObject = hologram.GetComponentInChildren<MeshFilter>().gameObject;
        hologramMeshObject.transform.localRotation = Quaternion.Euler(hologramRotation);
        hologram.transform.localPosition = hologramPosition;
        hologram.GetComponentInChildren<MeshFilter>().mesh = _itemMesh;
        hologram.GetComponentInChildren<Light>().color = hologramColor.gamma;
        hologram.GetComponentInChildren<Light>().intensity = hologramColor.maxColorComponent / 5;
        hologram.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetColor("_FresnelColor", hologramOutlineColor);
        hologram.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetColor("_MainColor", hologramColor);
        var mainModule = rayHologram.GetComponent<ParticleSystem>().main;
        mainModule.startColor = hologramColor;
        Debug.Log("validating hologram");
    }

    void Start()
    {
        _selectedItem = UnityEngine.Random.Range(0, eggAmount);
        initialY = hologram.transform.position.y;
        _itemTypes = Enum.GetValues(typeof(EasterEggItem.EasterEggs));
        //_selectedItem = UnityEngine.Random.Range(0, _itemTypes.Length);
        Egg.Egg = (EasterEggItem.EasterEggs)_itemTypes.GetValue(_selectedItem);
        _itemMesh = itemMeshes[_selectedItem].GetComponent<MeshFilter>()
            .sharedMesh; // /!\ meshes must be in the same order as in the enum
        holoScale = itemMeshes[_selectedItem].GetComponent<Transform>().localScale;
        //Debug.Log(holoScale);
    }

    private void Update()
    {
        if (!_reValidated)
        {
            _reValidated = true;
            OnValidate();
            if (_selectedItem == 0)
            {
                hologramMeshObject.transform.Rotate(Vector3.left, 90);
            }
        }

        if (amountChecked >= eggAmount) isLooted = true;
        if (!isOpen || isLooted)
        {
            hologram.SetActive(false);
            Egg.enabled = false;
            return;
        }

        if (_prevAmountChecked != amountChecked)
        {
            _prevAmountChecked = amountChecked;
            _itemMesh = itemMeshes[(_selectedItem + amountChecked) % eggAmount].GetComponent<MeshFilter>().sharedMesh;
            holoScale = itemMeshes[(_selectedItem + amountChecked) % eggAmount].GetComponent<Transform>().localScale;
            OnValidate();
            if ((_selectedItem + amountChecked) % eggAmount == 0)
            {
                hologramMeshObject.transform.Rotate(Vector3.left, 90);
            }
        }

        hologram.transform.localScale = holoScale;
        hologram.transform.Rotate(Vector3.up, hologramSpeedRotation * Time.deltaTime);
        var position = hologram.transform.position;
        hologram.transform.position = new Vector3(position.x,
            initialY + Mathf.Sin(Time.time * hologramSpeedMovement) * hologramAmplitudeMovement, position.z);
    }

    public void Open()
    {
        isOpen = true;
        hologram.SetActive(true);
        Egg.enabled = true;
    }

    public void Close()
    {
        isOpen = false;
        hologram.SetActive(false);
        Egg.enabled = false;
    }
}