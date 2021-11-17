using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private LoginPanel loginPanel;
        [SerializeField] private Image background;

        public async UniTask<StartGameResponse> ShowMenu()
        {
            _ = background.transform.DORotate(new Vector3(0f, 0f, 360f), 180f, RotateMode.FastBeyond360)
                .SetLoops(-1).SetEase(Ease.Linear).SetId("MenuWindow: background DORotate");
            loginPanel.Setup();
            await UniTask.WaitUntil(() => loginPanel.StartGameResponse != null);
            return loginPanel.StartGameResponse;
        }
    }
}