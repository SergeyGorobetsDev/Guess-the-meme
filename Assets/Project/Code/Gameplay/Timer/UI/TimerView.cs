using TMPro;
using UnityEngine;

namespace Assets.Project.Code.Gameplay.Timer.UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text timerText;

        private Timer timer;

        public void Initialize(Timer timer)
        {
            this.timer = timer;
        }

        private void Update()
        {
            if (timer == null || !timer.IsRunning || timer.IsPaused) return;
            timerText.text = timer.GetFormattedTime();
        }
    }
}
