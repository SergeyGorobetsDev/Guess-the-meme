using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem
{
    public sealed class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        private readonly string mainVolumeParam = "Volume";
        private readonly string sfxVolumeParam = "SFXVol";
        private readonly string uiVolumeParam = "UIVol";
        private readonly string musicVolumeParam = "MusicVol";

        [Header("Audio Components")]
        [SerializeField]
        public AudioSource musicSource;
        [SerializeField]
        public AudioMixer masterMixer;
        [SerializeField]
        public AudioMixerGroup sfxMixer;
        [SerializeField]
        public AudioMixerGroup uiMixer;
        [SerializeField]
        public AudioMixerGroup musicMixer;
        [SerializeField]
        public DefaultMixerTarget defaultMixer = DefaultMixerTarget.None;

        [Header("Audio Resources")]
        [SerializeField]
        private AudioBank soundBank;
        [SerializeField]
        private AudioBank musicBank;

        public void Initialization()
        {
            InitBanks();
            musicSource.outputAudioMixerGroup = musicMixer;
            InitMixer();
        }

        private void InitBanks()
        {
            soundBank.Build();
            musicBank.Build();
        }

        private void InitMixer()
        {
            SetMixerFromPref(mainVolumeParam);
            SetMixerFromPref(musicVolumeParam);
            SetMixerFromPref(sfxVolumeParam);
            SetMixerFromPref(uiVolumeParam);
        }

        public void Play(string clip, AudioMixerGroup mixerTarget, Vector3? position = null)
        {
            if (soundBank.TryGetAudio(clip, out AudioClip audioClip))
            {
                GameObject clipObj = new GameObject(clip, typeof(AudioDestroyer));
                AudioSource src = clipObj.AddComponent<AudioSource>();
                if (position.HasValue)
                {
                    clipObj.transform.position = position.Value;
                    src.spatialBlend = 1;
                    src.rolloffMode = AudioRolloffMode.Linear;
                    src.maxDistance = 50;
                    src.dopplerLevel = 0;
                }
                src.clip = audioClip;
                src.outputAudioMixerGroup = mixerTarget;
                src.Play();
            }
            else
            {
                Debug.LogWarning($"AudioClip '{clip}' not present in audio bank");
            }
        }

        public void Play(string clip, MixerTarget mixerTarget, Vector3? position = null) =>
            Play(clip, this.GetMixerGroup(mixerTarget), position);

        public void Play(string clip, string mixerTarget, Vector3? position = null) =>
            Play(clip, this.GetMixerGroup(mixerTarget), position);

        public void Play(string clip, Vector3? position = null) =>
            Play(clip, MixerTarget.Default, position);

        public void PlayAndFollow(string clip, Transform target, MixerTarget mixerTarget)
        {
            if (this.soundBank.TryGetAudio(clip, out AudioClip audioClip))
            {
                GameObject clipObj = new GameObject(clip, typeof(AudioDestroyer));
                AudioSource src = clipObj.AddComponent<AudioSource>();
                FollowTarget follow = clipObj.AddComponent<FollowTarget>();
                src.spatialBlend = 1;
                src.rolloffMode = AudioRolloffMode.Linear;
                src.maxDistance = 50;
                src.dopplerLevel = 0;
                src.clip = audioClip;
                src.outputAudioMixerGroup = this.GetMixerGroup(mixerTarget);
                follow.target = target;
                src.Play();
            }
            else
            {
                Debug.LogWarning($"AudioClip '{clip}' not present in audio bank");
            }
        }

        public void PlayAndFollow(string clip, Transform target) =>
            PlayAndFollow(clip, target, MixerTarget.Default);

        public void PlayMusic(string music)
        {
            if (string.IsNullOrEmpty(music) == false)
            {
                if (musicBank.TryGetAudio(music, out AudioClip audio))
                {
                    musicSource.outputAudioMixerGroup = musicMixer;
                    musicSource.clip = audio;
                    musicSource.Play();
                }
                else Debug.LogWarning($"AudioClip '{music}' not present in music bank");
            }
        }

        public void StopMusic()
        {
            musicSource.Stop();
            musicSource.clip = null;
        }

        public void SetVolumeMaster(float value)
        {
            masterMixer.SetFloat(mainVolumeParam, ToDecibels(value));
            SetPref(mainVolumeParam, value);
        }

        public void SetVolumeSFX(float value)
        {
            masterMixer.SetFloat(sfxVolumeParam, ToDecibels(value));
            SetPref(sfxVolumeParam, value);
        }

        public void SetVolumeUI(float value)
        {
            masterMixer.SetFloat(uiVolumeParam, ToDecibels(value));
            SetPref(uiVolumeParam, value);
        }

        public void SetVolumeMusic(float value)
        {
            masterMixer.SetFloat(musicVolumeParam, ToDecibels(value));
            SetPref(musicVolumeParam, value);
        }

        public float GetVolumeMaster() => GetPref(mainVolumeParam);
        public float GetVolumeSFX() => GetPref(sfxVolumeParam);
        public float GetVolumeUI() => GetPref(uiVolumeParam);
        public float GetVolumeMusic() => GetPref(musicVolumeParam);

        public float ToDecibels(float value) =>
            value == 0 ? -80 : Mathf.Log10(value) * 20;

        private float GetPref(string pref)
        {
            float v = PlayerPrefs.GetFloat(pref, 0.75f);
            return v;
        }

        private void SetPref(string pref, float val) =>
            PlayerPrefs.SetFloat(pref, val);

        private void SetMixerFromPref(string pref) =>
            masterMixer.SetFloat(pref, ToDecibels(GetPref(pref)));

        private AudioMixerGroup DefaultMixerGroup() =>
            GetMixerGroup((MixerTarget)defaultMixer);

        private AudioMixerGroup GetMixerGroup(MixerTarget target)
        {
            if (target == MixerTarget.None) return null;
            if (target == MixerTarget.Default) return GetMixerGroup((MixerTarget)defaultMixer);
            if (target == MixerTarget.SFX) return sfxMixer;
            if (target == MixerTarget.UI) return uiMixer;
            throw new System.Exception("Invalid MixerTarget");
        }

        private AudioMixerGroup GetMixerGroup(string target)
        {
            AudioMixerGroup[] foundGroups = masterMixer.FindMatchingGroups(target);
            if (foundGroups.Length > 0) return foundGroups[0];
            throw new System.Exception($"No mixer group by the name {target} could be found");
        }
    }
}