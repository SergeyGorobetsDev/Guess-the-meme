using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem
{
    public interface IAudioPlayer
    {
        public void Play(string clip, AudioMixerGroup mixerTarget, Vector3? position = null);
        public void Play(string clip, MixerTarget mixerTarget, Vector3? position = null);
        public void Play(string clip, string mixerTarget, Vector3? position = null);
        public void Play(string clip, Vector3? position = null);
        public void PlayAndFollow(string clip, Transform target, MixerTarget mixerTarget);
        void SetVolumeMaster(float value);
        void SetVolumeMusic(float value);
        void SetVolumeSFX(float value);
        void SetVolumeUI(float value);
        float GetVolumeMaster();
        float GetVolumeSFX();
        float GetVolumeUI();
        float GetVolumeMusic();
        void PlayMusic(string music);
        void Initialization();
        void StopMusic();
    }
}