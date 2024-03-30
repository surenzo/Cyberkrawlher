using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Chest : MonoBehaviour
{
    [Header("Hologram")]
    [SerializeField] private Mesh hologramMesh;
    [ColorUsage(true,true)] [SerializeField] private Color hologramColor;
    [ColorUsage(true,true)] [SerializeField] private Color hologramOutlineColor;
    
    
    [Header("Hologram transform settings")]
    [SerializeField] private Vector3 hologramSize = new Vector3(1, 1, 1);
    [SerializeField] private Vector3 hologramRotation = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 hologramPosition = new Vector3(0, 0, 0);
    
    [Header("Hologram movement settings")]
    [SerializeField] private float hologramSpeedRotation = 50f;
    [SerializeField] private float hologramAmplitudeMovement = 0.2f;
    [SerializeField] private float hologramSpeedMovement = 2f;
    
    [Header("Chest Settings")]
    [SerializeField] private bool isOpen = false;
    
    [Header("References")]
    [SerializeField] private GameObject hologram;
    [SerializeField] private GameObject rayHologram;

    private GameObject hologramMeshObject;
    

    private float initialY;
    private static readonly int FresnelColor = Shader.PropertyToID("FresnelColor");
    private static readonly int MainColor = Shader.PropertyToID("MainColor");

    private void OnValidate()
    {
        hologram.SetActive(isOpen);
        rayHologram.SetActive(isOpen);
        hologram.transform.localScale = hologramSize;
        hologramMeshObject = hologram.GetComponentInChildren<MeshFilter>().gameObject;
        hologramMeshObject.transform.localRotation = Quaternion.Euler(hologramRotation);
        hologram.transform.localPosition = hologramPosition;
        hologram.GetComponentInChildren<MeshFilter>().mesh = hologramMesh;
        hologram.GetComponentInChildren<Light>().color = hologramColor.gamma;
        hologram.GetComponentInChildren<Light>().intensity = hologramColor.maxColorComponent/5;
        hologram.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetColor("_FresnelColor", hologramOutlineColor); 
        hologram.GetComponentInChildren<MeshRenderer>().sharedMaterial.SetColor("_MainColor", hologramColor);
        var mainModule = rayHologram.GetComponent<ParticleSystem>().main;
        mainModule.startColor = hologramColor;
    }

    void Start()
    {
        OnValidate();
        initialY = hologram.transform.position.y;
    }
    
    private void Update()
    {
        if (!isOpen)
        {
            hologram.SetActive(false);
            return;
        }
        hologram.transform.localScale = hologramSize;
        hologram.transform.Rotate(Vector3.up, hologramSpeedRotation * Time.deltaTime);
        var position = hologram.transform.position;
        hologram.transform.position = new Vector3(position.x, initialY + Mathf.Sin(Time.time * hologramSpeedMovement) * hologramAmplitudeMovement, position.z);
    }
    
    public void Open()
    {
        isOpen = true;
        hologram.SetActive(true);
    }
    
    public void Close()
    {
        isOpen = false;
        hologram.SetActive(false);
    }
}
