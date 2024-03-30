using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityPool : MonoBehaviour
{
    public static EntityPool Instance;

    [SerializeField] private GameObject _lum;

    [SerializeField] private List<ShooterEntityBehaviour> ShooterPool = new List<ShooterEntityBehaviour>();
    [SerializeField] private List<BoxeEntityBehaviour> BoxerPool = new List<BoxeEntityBehaviour>();

    private List<ShooterEntityBehaviour> UsedShooterPool = new List<ShooterEntityBehaviour>();
    private List<BoxeEntityBehaviour> UsedBoxerPool = new List<BoxeEntityBehaviour>();


    private void Awake()
    {
        Instance = this;
    }

    public void MakeLum(int lumQuantity, Transform lumTransform)
    {
        Instantiate(_lum, lumTransform);
        _lum.GetComponent<LumItem>().lumQuantity = lumQuantity;
    }


    public void GoBack(AbstractEntityBehaviour entity)
    {
        if (entity == null) { return; }
        else
        {
            if (entity.Type == AbstractEntityBehaviour.entityType.shooter)
            {
                UsedShooterPool.Remove((ShooterEntityBehaviour)entity);
                ShooterPool.Add((ShooterEntityBehaviour)entity);
            }
            if (entity.Type == AbstractEntityBehaviour.entityType.shooter)
            {
                UsedBoxerPool.Remove((BoxeEntityBehaviour)entity);
                BoxerPool.Add((BoxeEntityBehaviour)entity);
            }
            entity.gameObject.SetActive(false);
            return;
        }
    }


    public void Make(AbstractEntityBehaviour.entityType type, Vector3 pos)
    {
        AbstractEntityBehaviour entity = null;
        if (type == AbstractEntityBehaviour.entityType.shooter)
        {
            if(ShooterPool.Count < 2)
            {
                GameObject go = new GameObject(ShooterPool[0].gameObject.name);
                entity = go.GetComponent<AbstractEntityBehaviour>();
                ShooterPool.Add((ShooterEntityBehaviour)entity);
            }
            else
            {
                 entity = ShooterPool[0];
            }
            ShooterPool.Remove((ShooterEntityBehaviour)entity);
            UsedShooterPool.Add((ShooterEntityBehaviour)entity);

        }
        if (type == AbstractEntityBehaviour.entityType.boxer)
        {
            if (BoxerPool.Count < 2)
            {
                GameObject go = new GameObject(ShooterPool[0].gameObject.name);
                entity = go.GetComponent<AbstractEntityBehaviour>();
                BoxerPool.Add((BoxeEntityBehaviour)entity);
            }
            else
            {
                entity = ShooterPool[0];
            }
            BoxerPool.Remove((BoxeEntityBehaviour)entity);
            UsedBoxerPool.Add((BoxeEntityBehaviour)entity);

        }
        entity.transform.position = pos;
        entity.gameObject.SetActive(true);
        return;

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Make(AbstractEntityBehaviour.entityType.shooter, Vector3.zero);
        }
    }

}
