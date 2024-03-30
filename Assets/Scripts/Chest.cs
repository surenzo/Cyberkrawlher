using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Chest : MonoBehaviour
{
    
    [SerializeField] private GameObject hologram;
    [SerializeField] private bool isOpen = false;

    private float initialY;
    
    void Start()
    {
        hologram.SetActive(isOpen);
        initialY = hologram.transform.position.y;
    }
    
    private void Update()
    {
        if (!isOpen) return;
        
        hologram.transform.Rotate(Vector3.up, 50 * Time.deltaTime);
        var position = hologram.transform.position;
        hologram.transform.position = new Vector3(position.x, initialY + Mathf.Sin(Time.time) * 0.2f, position.z);
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
