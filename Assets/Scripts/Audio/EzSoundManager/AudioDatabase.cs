using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace EzSoundManager
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Audio/Audio Database", order = 1)]
    public class AudioDatabase : ScriptableObject
    {
        private static AudioDatabase _instance;

        public List<SoundCategory> categories;

        public static AudioDatabase Instance {
            get {
                if (_instance.IsUnityNull())
                    _instance = Resources.Load<AudioDatabase>("AudioDatabase");
                return _instance;
            }
        }

        public void OnValidate()
        {
            categories.RemoveAll(category => category == null);
        }

        public AudioClip GetClipByNameAndCategory(string name, string categoryPath)
        {
            return GetClipInfoByNameAndCategory(name, categoryPath)?.clip;
        }


        public List<AudioClipInfo> GetClipInfosByCategory(string categoryPath)
        {
            Queue<SoundCategory> queue = new Queue<SoundCategory>(categories);

            while (queue.Count > 0)
            {
                SoundCategory category = queue.Dequeue();

                if (category.GetFullPath() == categoryPath)
                {
                    return category.Clips;
                }

                foreach (SoundCategory subCategory in category.SubCategories)
                {
                    queue.Enqueue(subCategory);
                }
            }

            Debug.LogWarning($"Audio category not found: {categoryPath}");
            return null;
        }
        public AudioClipInfo GetClipInfoByNameAndCategory(string name, string categoryPath)
        {
            Queue<SoundCategory> queue = new Queue<SoundCategory>(categories);

            while (queue.Count > 0)
            {
                SoundCategory category = queue.Dequeue();

                if (category.GetFullPath() == categoryPath)
                {
                    foreach (AudioClipInfo clipInfo in category.Clips.Where(clipInfo => clipInfo.clipName == name))
                    {
                        return clipInfo;
                    }
                }

                foreach (SoundCategory subCategory in category.SubCategories)
                {
                    queue.Enqueue(subCategory);
                }
            }

            Debug.LogWarning($"Audio clip not found: {name}");
            return null;
        }

        public List<AudioClip> GetClipsByCategory(string categoryPath)
        {
            Queue<SoundCategory> queue = new Queue<SoundCategory>(categories);

            while (queue.Count > 0)
            {
                SoundCategory category = queue.Dequeue();

                if (category.GetFullPath() == categoryPath)
                {
                    return category.Clips.Select(clipInfo => clipInfo.clip).ToList();
                }

                foreach (SoundCategory subCategory in category.SubCategories)
                {
                    queue.Enqueue(subCategory);
                }
            }

            Debug.LogWarning($"Audio category not found: {categoryPath}");
            return null;
        }
        public void AddCategory(SoundCategory soundCategory)
        {
            if (!categories.Contains(soundCategory))
            {
                categories.Add(soundCategory);
            }
        }
        public void RemoveCategory(SoundCategory soundCategory)
        {
            if (categories.Contains(soundCategory))
            {
                categories.Remove(soundCategory);
            }
        }
    }
}