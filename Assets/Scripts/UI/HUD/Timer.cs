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
                // Milliseconds also implemented but it is a bit expensive and not used currently
                //, GetMilliseconds(timePassed));
            }
        }

        public void StartTimer()
        {
            isRunning = true;
            lastTime = Time.realtimeSinceStartup;
            timePassed = 0f;
        }
        
        public void ResumeTimer()
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
            var timeString = AppendZeros(minutes.ToString(), 2);
            return timeString;
        }

        private string GetSecond(float time)
        {
            var seconds = (int) time % 60;
            var timeString = AppendZeros(seconds.ToString(), 2);
            return timeString;
        }
        
        
        private object GetMilliseconds(float time)
        {
            var milliseconds = (int) (time % 1 * 1000);
            var millisecondString = milliseconds.ToString();
            millisecondString = millisecondString.Substring(0, Mathf.Min(2, millisecondString.Length));
            var timeString = AppendZeros(millisecondString, 2);
            return timeString;
        }
        
        private static string AppendZeros(string timeString, int digits)
        {
            var zerosToAppend = digits - timeString.Length;
            var zeros = new string('0', zerosToAppend);
            timeString = zeros + timeString;

            return timeString;
        }
    }
}