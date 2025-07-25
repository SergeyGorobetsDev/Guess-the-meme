using System;
using UnityEngine;

namespace Assets.Project.Code.Gameplay.Timer
{
    public class Timer
    {
        [Header("Timer Settings")]
        [SerializeField]
        private float initialTime = 30f;
        [SerializeField]
        private bool countDown = true;

        public event Action OnTimerStart;
        public event Action OnTimerUpdate;
        public event Action OnTimerComplete;

        private float currentTime;
        private bool isRunning;
        private bool isPaused;

        public float CurrentTime => currentTime;
        public bool IsRunning => isRunning;
        public bool IsPaused => isPaused;
        public float Progress => countDown ? 1 - (currentTime / initialTime) : (currentTime / initialTime);

        public Timer()
        {

        }

        public void Tick()
        {
            if (!isRunning || isPaused) return;

            if (countDown)
            {
                currentTime -= Time.deltaTime;
                if (currentTime <= 0f)
                {
                    currentTime = 0f;
                    TimerComplete();
                }
            }
            else
            {
                currentTime += Time.deltaTime;
            }

            OnTimerUpdate?.Invoke();
        }

        public void StartTimer()
        {
            currentTime = countDown ? initialTime : 0f;
            isRunning = true;
            isPaused = false;
            OnTimerStart?.Invoke();
        }

        public void StartTimer(float customTime)
        {
            initialTime = customTime;
            StartTimer();
        }

        public void PauseTimer() =>
            isPaused = true;

        public void ResumeTimer() =>
            isPaused = false;

        public void StopTimer() =>
            isRunning = false;

        public void ResetTimer() =>
            currentTime = countDown ? initialTime : 0f;

        public void RestartTimer()
        {
            ResetTimer();
            StartTimer();
        }

        private void TimerComplete()
        {
            isRunning = false;
            OnTimerComplete?.Invoke();
        }

        public string GetFormattedTime(string format = @"mm\:ss")
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(currentTime);
            return timeSpan.ToString(format);
        }
    }
}
