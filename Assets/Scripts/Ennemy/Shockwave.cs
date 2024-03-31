using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Vector3 defaultScale = new Vector3(0, 0, 0);
    public float currentScale = 1;
    public float maxScale = 3;
    public float speed = 10;

    void Start()
    {
        defaultScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentScale += speed * Time.deltaTime;
        if (currentScale > maxScale)
        {
            currentScale = 1;
        }
        gameObject.SetActive(false);
    }
}