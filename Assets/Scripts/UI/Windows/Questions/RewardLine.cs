using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Windows
{
    public class RewardLine : MonoBehaviour
    {
        [SerializeField] private Image rewardIcon;
        [SerializeField] private AnimatedText rewardText;
        [SerializeField] private Image backgroundImage;
        
        [Header("Color of answers")]
        [SerializeField] private Color gainColor;
        [SerializeField] private Color lossColor;

        public async UniTask DisplaySingleRewardLine(int rewardAmount, Sprite rewardSprite)
        {
            transform.localScale = Vector3.zero;
            
            var format = rewardAmount > 0 ? "+{0}" : "{0}";
            
            rewardIcon.sprite = rewardSprite;
            rewardText.SetNewValue(0, format);
            backgroundImage.color = rewardAmount > 0 ? gainColor : lossColor;
            
            gameObject.SetActive(true);
            
            await transform.DOScale(Vector3.one, 0.25f);
            await rewardText.AnimateNewValue(rewardAmount, format);
            
            // todo animate reward

            await UniTask.Delay(500);
        }
    }
}