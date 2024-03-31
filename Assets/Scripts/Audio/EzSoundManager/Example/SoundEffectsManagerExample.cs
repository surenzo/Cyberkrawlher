using UnityEngine;

namespace EzSoundManager.Example
{
    public class SoundEffectsManagerExample : MonoBehaviour
    {
        // Utilisez des chemins complets pour les catégories
        [SerializeField] private string clickSoundCategoryPath = "Son/Effets Sonores/Click";
        [SerializeField] private string explosionSoundCategoryPath = "Son/Effets Sonores/Explosion";
        [SerializeField] private string ambientSoundCategoryPath = "Son/Effets Sonores/Ambient";

        [SerializeField] private string clickSoundName;
        [SerializeField] private string explosionSoundName;
        [SerializeField] private string ambientSoundName;
        [SerializeField] private GameObject targetGameObject;

        private void Update()
        {
            // Jouer un effet sonore "click" lors de l'appui sur la touche C
            if (Input.GetKeyDown(KeyCode.C))
            {
                SoundManager.Instance.PlaySoundEffect(clickSoundName, clickSoundCategoryPath, transform.position);
            }

            // Jouer un effet sonore "explosion" avec un fondu sur un GameObject spécifique lors de l'appui sur la touche E
            if (Input.GetKeyDown(KeyCode.E))
            {
                SoundManager.PlaySoundWithFadeOnGameObject(explosionSoundName, explosionSoundCategoryPath, targetGameObject, 2.0f);
            }

            // Jouer un effet sonore "ambient" sur le GameObject cible lors de l'appui sur la touche A
            if (Input.GetKeyDown(KeyCode.A))
            {
                SoundManager.PlaySoundOnGameObject(ambientSoundName, ambientSoundCategoryPath, targetGameObject);
            }

            // Jouer un effet sonore "click" avec un pitch aléatoire lors de l'appui sur la touche R
            if (Input.GetKeyDown(KeyCode.R))
            {
                SoundManager.Instance.RaiseSoundWithRandomPitch(clickSoundName, clickSoundCategoryPath, transform.position, 0.8f, 1.2f);
            }
        }
    }
}