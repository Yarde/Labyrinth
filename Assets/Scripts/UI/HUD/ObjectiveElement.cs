using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class ObjectiveElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private Image objectiveImage;
        [SerializeField] private Image cloudImage;

        private ObjectiveData _objectiveData;
        
        public void Setup(ObjectiveData objectiveData)
        {
            objectiveText.text = $"{objectiveData.Collected}/{objectiveData.Total}";
            objectiveImage.sprite = objectiveData.Prefab.Icon;
            cloudImage.color = objectiveData.Prefab.Color;

            _objectiveData = objectiveData;
        }

        private void Update()
        {
            objectiveText.text = $"{_objectiveData.Collected}/{_objectiveData.Total}";
        }
    }
}