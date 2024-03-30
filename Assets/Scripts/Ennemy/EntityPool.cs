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
        Debug.Log("Shooter Pool number : " + ShooterPool.Count);
    }


    private void IncrementPoolSize()
    {
        GameObject go = Instantiate(shooter);
        ShooterPool.Add(go);
        GameObject go2 = Instantiate(boxer);
        BoxerPool.Add(go2);
    }



    public void MakeLum(int lumQuantity, Vector3 lumTransform)
    {
        Debug.Log("Luuuumiere");
        GameObject lum = Instantiate(_lum, lumTransform, Quaternion.identity);
        lum.SetActive(true);
        lum.GetComponent<LumItem>().lumQuantity = lumQuantity;
        lum.transform.parent = lum.transform.parent.parent;

        Debug.Log(lum);
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
            entity = ShooterPool[0];

            BoxerPool.Remove(entity);
            UsedBoxerPool.Add(entity);
            entity.transform.position = pos;
            entity.gameObject.SetActive(true);
            return;
        }
    }


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
