using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Data", menuName = "Skill", order = 1)]
    public class SkillData : ScriptableObject 
    {
        public string name;
        public string displayName;
        public int maxLevel;
        public float bonusPerLevel;
        public int cost;
    }
}