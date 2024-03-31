using UnityEngine;

namespace EzSoundManager.Example
{
    public class MusicManagerExample : MonoBehaviour
    {

        private const float FadeDuration = 2.0f; // Durée du crossfade
        [SerializeField] private string[] backgroundMusicList;
        [SerializeField] private string categoryMusic;
        private int _currentMusicIndex;

        private void Start()
        {
            // Commencer par jouer la première musique de la liste
            PlayNextMusic();
        }

        private void Update()
        {
            // Exemple d'interaction (par exemple, lors d'un événement dans le jeu)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Arrêter la musique actuelle avec un fondu
                SoundManager.Instance.FadeOutMusic(FadeDuration);
            }

            // Exemple pour passer à la musique suivante avec un crossfade lorsque vous appuyez sur "S"
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlayNextMusicWithCrossfade();
            }
        }

        private void PlayNextMusic()
        {
            if (backgroundMusicList.Length > 0)
            {
                string nextMusic = backgroundMusicList[_currentMusicIndex];
                SoundManager.Instance.PlayMusicByCategory(nextMusic, categoryMusic);
                _currentMusicIndex = (_currentMusicIndex + 1) % backgroundMusicList.Length;
            }
        }

        private void PlayNextMusicWithCrossfade()
        {
            if (backgroundMusicList.Length > 0)
            {
                string nextMusic = backgroundMusicList[_currentMusicIndex];
                SoundManager.Instance.FadeOutToMusicToNewClip(nextMusic, categoryMusic, FadeDuration, 0.5f);
                _currentMusicIndex = (_currentMusicIndex + 1) % backgroundMusicList.Length;
            }
        }
    }
}