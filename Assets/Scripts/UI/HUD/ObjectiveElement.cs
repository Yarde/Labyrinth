using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ObjectiveElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private Image objectiveImage;

        private ObjectiveData _objectiveData;
        
        public void Setup(ObjectiveData objectiveData)
        {
            objectiveText.text = $"{objectiveData.Collected}/{objectiveData.Total}";
            objectiveImage.sprite = objectiveData.Prefab.Icon;

            _objectiveData = objectiveData;
        }

        private void Update()
        {
            objectiveText.text = $"{_objectiveData.Collected}/{_objectiveData.Total}";
        }
    }
}