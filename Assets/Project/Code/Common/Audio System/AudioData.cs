using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Game / Audio / AudioData", order = 1)]
    public class AudioData : ScriptableObject
    {
        [Header("Audio Resources")]
        [field: SerializeField]
        public AudioBank SoundBank { get; private set; }
        [field: SerializeField]
        public AudioBank MusicBank { get; private set; }

        [Header("Components")]
        [field: SerializeField]
        public AudioMixer MasterMixer { get; private set; }
    }
}