using LightCurrencySystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DarkInvisibility : MonoBehaviour
{
    [SerializeField] private GameObject goSkinned;
    private Animator skinAnim;
    [SerializeField] private GameObject goGhost;
    private Animator ghostAnim;
    private RaycastHit _raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        goGhost.SetActive(true);
        goSkinned.SetActive(!goGhost.activeSelf);
        skinAnim = goSkinned.GetComponent<Animator>();
        ghostAnim = goGhost.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        Vector3 dir = other.transform.position - transform.position;
        if(other.gameObject.layer == 7)
        {
            Debug.Log("in litArea");

            Physics.Raycast(transform.position, dir, out _raycastHit, other.transform.GetComponent<LightableObjects>().lightCost / 20f);
            if(_raycastHit.transform.gameObject.layer == 7)
            {
                Debug.Log("light");
                goGhost.SetActive(false);
                goSkinned.SetActive(true);
            }
            else
            {
                Debug.Log("dark");

                goGhost.SetActive(true);
                goSkinned.SetActive(false);
            }
        }
        else
        {
            Debug.Log("dark");

            goGhost.SetActive(true);
            goSkinned.SetActive(false);
        }

    }


    private void OnTriggerExit(Collider other)
    {
        goGhost.SetActive(true);
        goSkinned.SetActive(false);
    }
}
