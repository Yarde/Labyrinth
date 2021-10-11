using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ExperienceBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshPro experienceText;

        private int currentExperience;
        private void UpdateBar(int experience)
        {
            if (currentExperience < experience)
            {
                // todo animate experience gain
                experienceText.text = experience.ToString();
                currentExperience = experience;
            }
        }
    }
}