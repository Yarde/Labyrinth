using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Questions
{
    public class RewardPopup : MonoBehaviour
    {
        [SerializeField] private RewardLine rewardLinePrefab;
        [SerializeField] private Transform rewardHolder;
        [SerializeField] protected Button confirmButton;

        [Header("Reward Sprites")] 
        [SerializeField] private Sprite coinSprite;
        [SerializeField] private Sprite experienceSprite;
        [SerializeField] private Sprite heartSprite;

        private bool _confirmed;
        private Dictionary<RewardType, RewardLine> _rewardLines;
        private CancellationTokenSource _cancelToken;

        public void SetupRewardPopup()
        {
            _rewardLines = new Dictionary<RewardType, RewardLine>();
            confirmButton.onClick.AddListener(() => _confirmed = true);
        }
        
        public async UniTask DisplayRewards(QuestionResult result)
        {
            _confirmed = false;
            _cancelToken = new CancellationTokenSource();
            
            AnimateRewards(result).WithCancellation(_cancelToken.Token);
            await UniTask.WaitUntil(() => _confirmed).WithCancellation(_cancelToken.Token);

            _cancelToken.Cancel();
            _cancelToken.Dispose();
            _cancelToken = null;
            
            foreach (var line in _rewardLines)
            {
                line.Value.gameObject.SetActive(false);
            }
        }

        private async UniTask AnimateRewards(QuestionResult result)
        {
            if (result.Coins != 0)
            {
                AddLineIfNotExists(RewardType.Coins);
                await AnimateReward(RewardType.Coins, result.Coins, coinSprite);
            }

            if (result.Experience != 0)
            {
                AddLineIfNotExists(RewardType.Experience);
                await AnimateReward(RewardType.Experience, result.Experience, experienceSprite);
            }

            if (result.Hearts != 0)
            {
                AddLineIfNotExists(RewardType.Hearts);
                await AnimateReward(RewardType.Hearts, result.Hearts, heartSprite);
            }
        }

        private void AddLineIfNotExists(RewardType rewardType)
        {
            if (_rewardLines.ContainsKey(rewardType)) 
                return;
            
            var rewardLine = Instantiate(rewardLinePrefab, rewardHolder);
            rewardLine.gameObject.SetActive(false);
            _rewardLines[rewardType] = rewardLine;
        }
        
        private async UniTask AnimateReward(RewardType rewardType, int amount, Sprite sprite)
        {
            await _rewardLines[rewardType].DisplaySingleRewardLine(amount, sprite).WithCancellation(_cancelToken.Token);
        }
    }

    public enum RewardType
    {
        Coins = 0,
        Experience = 1,
        Hearts = 2
    }
}