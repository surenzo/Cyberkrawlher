using LightCurrencySystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DarkInvisibility : MonoBehaviour
{
    [SerializeField] private GameObject goSkinned;
    [SerializeField] private GameObject goGhost;
    private RaycastHit _raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        goGhost.SetActive(!goSkinned.activeSelf);
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 dir = other.transform.position - transform.position;
        if(other.gameObject.layer == 3)
        {
            Physics.Raycast(transform.position, dir, out _raycastHit, other.transform.GetComponent<LightableObjects>().lightCost / 20f);
            if(_raycastHit.transform.gameObject.layer == 3 || _raycastHit.transform.gameObject.layer == 7)
            {
                goGhost.SetActive(false);
                goSkinned.SetActive(true);
            }
            else
            {
                goGhost.SetActive(true);
                goSkinned.SetActive(false);
            }
        }
        else
        {
            goGhost.SetActive(true);
            goSkinned.SetActive(false);
        }

    }
}
