using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private string timeFormat = "{0}:{1}";

        private float startTime;

        private void Update()
        {
            var time = Time.realtimeSinceStartup - startTime;
            timeText.text = string.Format(timeFormat, GetMinute(time), GetSecond(time));
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

        public void Setup()
        {
            startTime = Time.realtimeSinceStartup;
        }
    }
}