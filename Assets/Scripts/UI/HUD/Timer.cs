using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private string timeFormat = "{0}:{1}";

        private bool isRunning;
        private float lastTime;
        private float timePassed;

        private void Update()
        {
            if (isRunning)
            {
                timePassed += Time.realtimeSinceStartup - lastTime;
                lastTime = Time.realtimeSinceStartup;
                timeText.text = string.Format(timeFormat, GetMinute(timePassed), GetSecond(timePassed));
            }
        }

        public void StartTimer()
        {
            isRunning = true;
            lastTime = Time.realtimeSinceStartup;
        }
        
        public void StopTimer()
        {
            isRunning = false;
        }

        private string GetMinute(float time)
        {
            var minutes = (int) time / 60;
            if (minutes > 9)
            {
                return minutes.ToString();
            }

            return minutes > 0 ? $"0{minutes}" : "00";
        }
        
        private string GetSecond(float time)
        {
            var seconds = (int) time % 60;
            return seconds > 9 ? seconds.ToString() : $"0{seconds}";
        }
    }
}