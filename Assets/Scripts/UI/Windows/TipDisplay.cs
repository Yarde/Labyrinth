using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Windows
{
    public class TipDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tipText;

        public async UniTask DisplayTip(string tip, int tipTime = 3000)
        {
            tipText.text = tip;
            tipText.transform.localScale = Vector3.one * 2;
            gameObject.SetActive(true);
            await tipText.transform.DOScale(Vector3.one, 0.5f);
            await UniTask.Delay(tipTime);
            await tipText.transform.DOScale(Vector3.zero, 0.5f);
            gameObject.SetActive(false);
        }
    }
}