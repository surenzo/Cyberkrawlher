using UnityEngine;

namespace EzSoundManager.Example
{
    public class IntroMusicPlayer : MonoBehaviour
    {
        [SerializeField] private string introMusicName; // Nom du clip d'introduction
        [SerializeField] private string mainMusicName; // Nom du clip principal
        [SerializeField] private string musicCategory; // Catégorie des clips musicaux

        private void Start()
        {
            // Jouer la musique avec l'introduction au démarrage
            SoundManager.Instance.PlayMusicWithIntro(introMusicName, mainMusicName, musicCategory);
        }

        // Exemple de méthode pour arrêter la musique
        public void StopMusic()
        {
            SoundManager.Instance.StopMusic();
        }

        // Option pour changer la musique en cours de jeu
        public void ChangeMusic(string newIntroName, string newMainName)
        {
            SoundManager.Instance.PlayMusicWithIntro(newIntroName, newMainName, musicCategory);
        }
    }
}