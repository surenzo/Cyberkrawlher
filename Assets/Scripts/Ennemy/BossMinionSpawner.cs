using UnityEngine;

namespace Ennemy
{
    public class BossMinionSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnRange;

        private int _toSpawn;

        private void Start()
        {
            _toSpawn = EntityPool.BoxerToSpawnWithBoss + EntityPool.ShooterToSpawnWithBoss;
        }

        private void Update()
        {
            _toSpawn = EntityPool.BoxerToSpawnWithBoss + EntityPool.ShooterToSpawnWithBoss;
            if (_toSpawn != 0)
            {
                spawnMultipleAtOnce();
            }
        }

        public void SpawnEntityAtPlace(Vector3 pos, AbstractEntityBehaviour.entityType type)
        {
            EntityPool.Instance.Make(type, pos);
        }

        public void spawnMultipleAtOnce()
        {
            if (_toSpawn <= 0) return;
            if (_toSpawn == 1)
            {
                if (EntityPool.BoxerToSpawnWithBoss > 0)
                {
                    SpawnEntityAtPlace(transform.position + spawnRange * new Vector3(0, 0, 1),
                        AbstractEntityBehaviour.entityType.boxer);
                    EntityPool.BoxerToSpawnWithBoss = 0;
                }
                else
                {
                    SpawnEntityAtPlace(transform.position + spawnRange * new Vector3(0, 0, 1),
                        AbstractEntityBehaviour.entityType.shooter);
                    EntityPool.ShooterToSpawnWithBoss = 0;
                }
            }
            else
            {
                float theta = 2 * Mathf.PI / _toSpawn;
                for (int i = 0; i < _toSpawn; ++i)
                {
                    Vector3 pos = transform.position +
                                  spawnRange * new Vector3(Mathf.Cos(i * theta), 0, Mathf.Sin(i * theta));
                    AbstractEntityBehaviour.entityType spawnType =
                        EntityPool.BoxerToSpawnWithBoss > EntityPool.ShooterToSpawnWithBoss
                            ? AbstractEntityBehaviour.entityType.boxer
                            : AbstractEntityBehaviour.entityType.shooter;
                    SpawnEntityAtPlace(pos, spawnType);
                    if (spawnType == AbstractEntityBehaviour.entityType.boxer)
                    {
                        EntityPool.BoxerToSpawnWithBoss -= 1;
                    }
                    else
                    {
                        EntityPool.ShooterToSpawnWithBoss -= 1;
                    }
                }
            }
        }
    }
}