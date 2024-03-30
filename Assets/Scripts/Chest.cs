using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Chest : MonoBehaviour
{
    
    [SerializeField] private GameObject hologram;
    [SerializeField] private GameObject rayHologram;
    [SerializeField] private Vector3 hologramSize = new Vector3(1, 1, 1);
    [SerializeField] private bool isOpen = false;
    [SerializeField] private float hologramSpeedRotation = 50f;
    [SerializeField] private float hologramAmplitudeMovement = 0.2f;
    [SerializeField] private float hologramSpeedMovement = 2f;
    

    private float initialY;

    private void OnValidate()
    {
        hologram.SetActive(isOpen);
        rayHologram.SetActive(isOpen);
        hologram.transform.localScale = hologramSize;
    }

    void Start()
    {
        hologram.SetActive(isOpen);
        rayHologram.SetActive(isOpen);
        initialY = hologram.transform.position.y;
        hologram.transform.localScale = hologramSize;
        rayHologram.GetComponent<ParticleSystem>().Emit(1);
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
