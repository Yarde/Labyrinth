using Cysharp.Threading.Tasks;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] private RectTransform cutoutTransform;
        [SerializeField] private Transform arrowTransform;
        [SerializeField] private TextMeshProUGUI tipText;
        [SerializeField] private Button skipButton;
        
        [SerializeField] private TutorialTipData[] tipsData;

        private bool _wasSkipped;

        public async UniTask DisplayTutorial()
        {
            skipButton.onClick.AddListener(() => _wasSkipped = true);
            
            foreach (var tip in tipsData)
            {
                tipText.transform.localPosition = tip.tipPosition;
                tipText.text = tip.tipText;
                cutoutTransform.localPosition = tip.cutoutPosition;
                cutoutTransform.sizeDelta = tip.cutoutSize;
                arrowTransform.eulerAngles = arrowTransform.eulerAngles.WithZ(tip.arrowRotationZ);

                await UniTask.WaitUntil(() => _wasSkipped);
                _wasSkipped = false;
            }
        }
    }
}