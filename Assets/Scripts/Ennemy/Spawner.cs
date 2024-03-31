using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float shooterProbability;
    [SerializeField] private float boxerProbability;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnRange;
    [SerializeField] private int playerDetectionDistance;

    [SerializeField] private float spawnDelay;
    private float spawnTimer;

    private bool hasSpawned = false;


    public void spawnEntityAtPlace(Vector3 pos)
    {
        shooterProbability /= (shooterProbability + boxerProbability);
        boxerProbability /= (shooterProbability + boxerProbability);

        float r = Random.value;
        if (r < shooterProbability) EntityPool.Instance.Make(AbstractEntityBehaviour.entityType.shooter, pos);
        else if (r <= 1) EntityPool.Instance.Make(AbstractEntityBehaviour.entityType.boxer, pos);     //pour ajouter des ennemis
    }

    public void spawnMultipleAtOnce()
    {
        if (spawnCount <= 0) return;
        if (spawnCount == 1) spawnEntityAtPlace(transform.position);
        else
        {   
            float theta = 2*Mathf.PI / spawnCount;
            for (int i = 0; i < spawnCount; ++i)
            {
                Vector3 pos = transform.position + spawnRange * new Vector3(Mathf.Cos(i*theta),0, Mathf.Sin(i * theta));
                spawnEntityAtPlace(pos);
            }
        }
    }


    // spawnDelay <=0 fera spawn les ennemis qu'une seule fois
    void Update()
    {
        if(spawnDelay > 0)
        {
            if (spawnTimer < 0)
            {
                hasSpawned = false;
            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }
        }
        
        if (!hasSpawned && Vector3.Distance(FPSController.Instance.transform.position,transform.position) < playerDetectionDistance)
        {
            spawnMultipleAtOnce();
            hasSpawned = true;
            spawnTimer = spawnDelay;
        }


    }


}
