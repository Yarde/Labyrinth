using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;

namespace Menu
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private LoginPanel loginPanel;

        public async UniTask<StartGameResponse> ShowMenu()
        {
            loginPanel.Setup();
            await UniTask.WaitUntil(() => loginPanel.Data != null);
            return loginPanel.Data;
        }
    }
}