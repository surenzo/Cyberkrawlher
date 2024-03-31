using LightCurrencySystem;
using UnityEngine;

namespace Ennemy
{
    public class BossManager : MonoBehaviour
    {
        [SerializeField] private LightableObjects firstPhase;
        [SerializeField] private LightableObjects secondPhase;
        private ShooterEntityBehaviour shooter;

        void Start()
        {
            MakeFirstPhaseBoss();

            MakeSecondPhaseBoss();
        
        }

        // Update is called once per frame
        void Update()
        {
            if(firstPhase.isLitUp)
            {
                firstPhase.gameObject.SetActive(false);
                secondPhase.gameObject.SetActive(true);
            }
            if(!firstPhase.gameObject.activeSelf && secondPhase.isLitUp)
            {
                firstPhase.gameObject.SetActive(false) ;
                secondPhase.gameObject.SetActive(true) ;
            }
        }



        private void MakeFirstPhaseBoss()
        {
            GameObject shooterGO = EntityPool.Instance.Make(AbstractEntityBehaviour.entityType.shooter, firstPhase.transform.position);
            shooterGO.transform.parent = firstPhase.transform;
            shooter = shooterGO.GetComponent<ShooterEntityBehaviour>();
            shooter._speed = 0;
            shooter.agent.speed = 0;
            shooter._attackFrequency = 0.3f;
            shooter.chargeur = 1;
            shooterGO.transform.GetChild(1).gameObject.SetActive(false);

            firstPhase.gameObject.SetActive(true);

        }

        private void MakeSecondPhaseBoss()
        {
            GameObject shooterGO = EntityPool.Instance.Make(AbstractEntityBehaviour.entityType.shooter, firstPhase.transform.position);
            shooterGO.transform.parent = firstPhase.transform;
            shooter = shooterGO.GetComponent<ShooterEntityBehaviour>();
            shooter._speed = 0;
            shooter.agent.speed = 0;
            shooter._attackFrequency = 0.15f;
            shooter.chargeur = 1;
            shooterGO.transform.GetChild(1).gameObject.SetActive(false);

            secondPhase.gameObject.SetActive(false);
        }

    }
}