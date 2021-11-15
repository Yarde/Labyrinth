using TMPro;
using UnityEngine;

namespace Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string id;
        private TextMeshProUGUI text;
        public void Awake()
        {
            TranslationManager.OnLanguageChanged += Translate;
            text = GetComponent<TextMeshProUGUI>();

            Translate();
        }

        private void Translate()
        {
            text.text = TranslationManager.Instance.GetTranslation(id);
        }
    }
}