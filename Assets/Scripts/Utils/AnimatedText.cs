using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimatedText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private TextMeshProUGUI Text
        {
            get
            {
                if (!text)
                {
                    text = gameObject.GetComponent<TextMeshProUGUI>();
                }

                return text;
            }
        }

        private int _currentValue;
        private string _textToFormat;

        private const float DELAY = 0.01f;
        
        public void SetNewValue(int finalValue, string textToFormat = "{0}")
        {
            Text.text = string.Format(textToFormat, finalValue);

            _currentValue = finalValue;
        }

        public async UniTask AnimateNewValue(int finalValue, string textToFormat = "{0}", float duration = 1.0f)
        {
            var startTime = Time.realtimeSinceStartup;
            var timePassed = 0f;

            transform.DOShakeRotation(duration, new Vector3(0f, 0f, 20f));
            while (Time.realtimeSinceStartup < startTime + duration)
            {
                timePassed += DELAY;
                await UniTask.Delay((int) (DELAY * 1000));
                
                var newValue = (int) ((_currentValue + finalValue) * (timePassed / duration));
                Text.text = string.Format(textToFormat, _currentValue + newValue);
            }

            SetNewValue(finalValue, textToFormat);
        }
    }
}