using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityPool : MonoBehaviour
{
    public static EntityPool Instance;

    [SerializeField] private GameObject _lum;

    [SerializeField] private GameObject shooter;
    private List<GameObject> ShooterPool = new List<GameObject>();
    private List<GameObject> UsedShooterPool = new List<GameObject>();


    [SerializeField] private GameObject boxer;
    private List<GameObject> BoxerPool = new List<GameObject>();
    private List<GameObject> UsedBoxerPool = new List<GameObject>();


    private void Awake()
    {
        Instance = this;

        for(int i = 0; i < 30; i++)
        {
            IncrementPoolSize();
        }
    }


    private void IncrementPoolSize()
    {
        GameObject go = Instantiate(shooter);
        ShooterPool.Add(go);
        GameObject go2 = Instantiate(boxer);
        BoxerPool.Add(go2);
    }



    public void MakeLum(Vector3 lumPos)
    {
        GameObject lum = Instantiate(_lum, lumPos + 0.75f*Vector3.up, Quaternion.identity);
        lum.SetActive(true);
        Item lumItem = lum.GetComponent<Item>();

        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0: lumItem.effect = Item.ItemEffects.LightLightRegen; break;
            case 1: lumItem.effect = Item.ItemEffects.MedLightRegen; break;
            case 2: lumItem.effect = Item.ItemEffects.BigLightRegen; break;
        }
    }


    public void GoBack(GameObject entity)
    {
        entity.SetActive(false);

        if (entity.GetComponent<AbstractEntityBehaviour>().Type == AbstractEntityBehaviour.entityType.shooter)
        {
            UsedShooterPool.Remove(entity);
            ShooterPool.Add(entity);  
        }
        if (entity.GetComponent<AbstractEntityBehaviour>().Type == AbstractEntityBehaviour.entityType.boxer)
        {
            UsedBoxerPool.Remove(entity);
            BoxerPool.Add(entity);
        }
        return;
    }


    public void Make(AbstractEntityBehaviour.entityType type, Vector3 pos)
    {
        if(ShooterPool.Count < 2 || BoxerPool.Count < 2)
        {
            for (int i = 0; i < 30; i++)
            {
                IncrementPoolSize();
            }
        }

        GameObject entity;
        if (type == AbstractEntityBehaviour.entityType.shooter)
        {
            entity = ShooterPool[0];
            
            ShooterPool.Remove(entity);
            UsedShooterPool.Add(entity);
            entity.transform.position = pos;
            entity.gameObject.SetActive(true);
            return;
        }
        if (type == AbstractEntityBehaviour.entityType.boxer)
        {
            entity = BoxerPool[0];

            BoxerPool.Remove(entity);
            UsedBoxerPool.Add(entity);
            entity.transform.position = pos;
            entity.gameObject.SetActive(true);
            return;
        }
    }


    /* ============== DEBUG POOL =============== */
    
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Make(AbstractEntityBehaviour.entityType.shooter, Vector3.zero);
            Debug.Log("made");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            UsedShooterPool[0].GetComponent<ShooterEntityBehaviour>().Damage(5, FPSController.Instance.transform, 1);
            Debug.Log("killed");
        }
    }
    
}
