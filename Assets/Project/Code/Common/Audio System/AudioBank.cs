using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem
{
    [System.Serializable]
    public class AudioBank
    {
        [SerializeField]
        private BankKVP[] kvps;

        private readonly Dictionary<string, AudioClip> dictionary = new Dictionary<string, AudioClip>();

        public bool Validate()
        {
            if (kvps.Length == 0) return false;

            List<string> keys = new List<string>();
            foreach (var kvp in kvps)
            {
                if (keys.Contains(kvp.Key)) return false;
                keys.Add(kvp.Key);
            }
            return true;
        }

        public void Build()
        {
            if (Validate())
            {
                for (int i = 0; i < kvps.Length; i++)
                {
                    dictionary.Add(kvps[i].Key, kvps[i].Value);
                }
            }
        }

        public bool TryGetAudio(string key, out AudioClip audio)
        {
            return dictionary.TryGetValue(key, out audio);
        }
    }
}