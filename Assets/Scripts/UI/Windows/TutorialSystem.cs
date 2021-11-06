using TMPro;
using UI.Windows;
using UnityEngine;

namespace UI
{
    public class TutorialSystem : MonoBehaviour
    {
        [SerializeField] private RectTransform cutoutTransform;
        [SerializeField] private Transform arrowTransform;
        [SerializeField] private TextMeshProUGUI tipText;
        
        [SerializeField] private TutorialTipData[] tipsData;
    }
}