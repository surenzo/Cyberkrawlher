using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Chest : MonoBehaviour
{
    
    [SerializeField] private GameObject hologram;
    [SerializeField] private GameObject rayHologram;
    [SerializeField] private Mesh hologramMesh;
    [SerializeField] private Vector3 hologramSize = new Vector3(1, 1, 1);
    [SerializeField] private Vector3 hologramRotation = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 hologramPosition = new Vector3(0, 0, 0);
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float hologramSpeedRotation = 50f;
    [SerializeField] private float hologramAmplitudeMovement = 0.2f;
    [SerializeField] private float hologramSpeedMovement = 2f;

    private GameObject hologramMeshObject;
    

    private float initialY;

    private void OnValidate()
    {
        hologram.SetActive(isOpen);
        rayHologram.SetActive(isOpen);
        hologram.transform.localScale = hologramSize;
        hologramMeshObject = hologram.GetComponentInChildren<MeshFilter>().gameObject;
        hologramMeshObject.transform.localRotation = Quaternion.Euler(hologramRotation);
        hologram.transform.localPosition = hologramPosition;
        hologram.GetComponentInChildren<MeshFilter>().mesh = hologramMesh;
        
    }

    void Start()
    {
        hologram.SetActive(isOpen);
        rayHologram.SetActive(isOpen);
        hologramMeshObject = hologram.GetComponentInChildren<MeshFilter>().gameObject;
        initialY = hologram.transform.position.y;
        hologram.transform.localScale = hologramSize;
        hologramMeshObject.transform.localRotation = Quaternion.Euler(hologramRotation);
        hologram.transform.position = hologramPosition;
        hologram.GetComponentInChildren<MeshFilter>().mesh = hologramMesh;
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
