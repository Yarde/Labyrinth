using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Menu
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private LoginPanel loginPanel;

        public async UniTask<object> ShowMenu()
        {
            loginPanel.Setup();
            await UniTask.WaitUntil(() => loginPanel.Data != null);
            return loginPanel.Data;
        }
    }
}