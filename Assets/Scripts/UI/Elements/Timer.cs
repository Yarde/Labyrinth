using TMPro;
using UnityEngine;

namespace UI.Elements
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private string timeFormat = "{0}:{1}";

        private bool _isRunning;
        private float _lastTime;
        private float _timePassed;
        public int ElapsedSeconds => (int) _timePassed;

        private void Update()
        {
            if (_isRunning)
            {
                _timePassed += Time.realtimeSinceStartup - _lastTime;
                _lastTime = Time.realtimeSinceStartup;
                timeText.text = string.Format(timeFormat, GetMinute(_timePassed), GetSecond(_timePassed));
                // Milliseconds also implemented but it is a bit expensive and not used currently
                //, GetMilliseconds(timePassed));
            }
        }

        public void StartTimer()
        {
            _isRunning = true;
            _lastTime = Time.realtimeSinceStartup;
            _timePassed = 0f;
        }
        
        public void ResumeTimer()
        {
            _isRunning = true;
            _lastTime = Time.realtimeSinceStartup;
        }
        
        public void StopTimer()
        {
            _isRunning = false;
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