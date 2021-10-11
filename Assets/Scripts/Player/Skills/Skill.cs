using UnityEngine;

namespace UI
{
    public abstract class Skill : MonoBehaviour
    {
        [SerializeField] protected string skillName;
        [SerializeField] protected int maxLevel;
        [SerializeField] protected float bonusPerLevel;
        [SerializeField] protected int cost;

        public abstract void Upgrade();
    }
}