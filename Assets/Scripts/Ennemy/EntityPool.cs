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



    public void MakeLum(int lumQuantity, Transform lumTransform)
    {
        Instantiate(_lum, lumTransform);
        _lum.GetComponent<LumItem>().lumQuantity = lumQuantity;
    }


    public void GoBack(GameObject entity)
    {
        Debug.Log("pool size : " + ShooterPool.Count);
        Debug.Log("usedPool size" + UsedShooterPool.Count);
        if (entity == null) { return; }
        else
        {
            if (entity.GetComponent<AbstractEntityBehaviour>().Type == AbstractEntityBehaviour.entityType.shooter)
            {
                UsedShooterPool.Remove(entity.gameObject);
                ShooterPool.Add(entity.gameObject);
            }
            if (entity.GetComponent<AbstractEntityBehaviour>().Type == AbstractEntityBehaviour.entityType.boxer)
            {
                UsedBoxerPool.Remove(entity.gameObject);
                BoxerPool.Add(entity.gameObject);
            }
            entity.gameObject.SetActive(false);
            Debug.Log("pool size : " + ShooterPool.Count);
            Debug.Log("usedPool size" + UsedShooterPool.Count);
            return;
        }
    }


    public void Make(AbstractEntityBehaviour.entityType type, Vector3 pos)
    {
        Debug.Log("Current pool size : " + ShooterPool.Count);
        if(ShooterPool.Count < 2 || BoxerPool.Count < 2)
        {
            for (int i = 0; i < 30; i++)
            {
                IncrementPoolSize();
            }
            Debug.Log("new pool size : " + ShooterPool.Count);
        }

        


        GameObject entity;
        if (type == AbstractEntityBehaviour.entityType.shooter)
        {
            entity = ShooterPool[0];
            
            ShooterPool.Remove(entity);
            UsedShooterPool.Add(entity);
            entity.transform.position = pos;
            entity.gameObject.SetActive(true);
            Debug.Log("pool size after spawned : " + ShooterPool.Count);
            Debug.Log("usedPool size" + UsedShooterPool.Count);
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
            GoBack(ShooterPool[0]);
            Debug.Log("killed");
        }
    }

}
