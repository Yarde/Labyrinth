﻿using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Data", menuName = "Skill", order = 1)]
    public class SkillData : ScriptableObject 
    {
        public string skillName;
        public string displayName;
        public int maxLevel;
        public float bonusPerLevel;
        public int startingCost;
    }
}