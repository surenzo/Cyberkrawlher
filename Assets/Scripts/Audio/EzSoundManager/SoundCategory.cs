using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace EzSoundManager
{
    [CreateAssetMenu(fileName = "SoundCategory", menuName = "Audio/Sound Category", order = 2)]
    public class SoundCategory : ScriptableObject
    {
        [SerializeField] private List<AudioClipInfo> clips;
        [SerializeField] private List<SoundCategory> subCategories;

        public SoundCategory parentCategory;

        public List<AudioClipInfo> Clips {
            get {
                return clips;
            }
        }
        public List<SoundCategory> SubCategories {
            get {
                return subCategories;
            }
        }

        private void OnEnable()
        {
            if (parentCategory == null) AudioDatabase.Instance.AddCategory(this);
            else AudioDatabase.Instance.RemoveCategory(this);
        }

        private void OnValidate()
        {
            SetParentForSubCategories();

            ValidateParentCategory();

            ValidateClips();

            ValidateDuplicate();
        }

        public string GetFullPath()
        {
            if (parentCategory.IsUnityNull())
            {
                return name;
            }
            return parentCategory.GetFullPath() + "/" + name;
        }

        private void ValidateDuplicate()
        {

            for (int i = 0; i < clips.Count; i++)
            {
                for (int j = i + 1; j < clips.Count; j++)
                {
                    if (clips[i].clipName == clips[j].clipName)
                    {
                        Debug.LogWarning($"Duplicate clip name in category {GetFullPath()}: {clips[i].clipName}");
                    }
                }
            }
        }
        private void ValidateClips()
        {

            foreach (AudioClipInfo clip in clips.Where(clip => clip != null))
            {
                clip.clipName = clip.clipName.Trim();
                clip.volume = Mathf.Clamp01(clip.volume);
                if (clip.clip == null)
                {
                    Debug.LogWarning($"Clip not found in category {GetFullPath()}: {clip.clipName}");
                }
            }
        }

        private void SetParentForSubCategories()
        {
            foreach (SoundCategory subCategory in subCategories.Where(subCategory => subCategory != null))
            {
                subCategory.parentCategory = this;
                subCategory.OnValidate();
            }
        }

        private void ValidateParentCategory()
        {
            if (parentCategory != null && !parentCategory.SubCategories.Contains(this))
            {
                parentCategory = null;
            }
        }
    }
}