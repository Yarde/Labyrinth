using UnityEngine;

namespace Gameplay.Skills
{
    public abstract class Skill
    {
        protected readonly Player Player;
        public readonly SkillData Data;
        public int Level;
        public int Cost;
        public float CompletionPercentage => Level / (float) Data.maxLevel;

        protected Skill(Player player, SkillData data)
        {
            Player = player;
            Data = data;
            Level = 0;
            Cost = Data.startingCost;
        }
        
        public virtual void Upgrade()
        {
            if (Player.Coins < Cost)
            {
                Debug.Log($"Not enough coins, player has {Player.Coins} but {Cost} is required for skill {Data.skillName}");
                return;
            }
            
            if (Level == Data.maxLevel)
            {
                Debug.Log($"Already max level {Data.maxLevel} for skill {Data.skillName}");
                return;
            }

            Player.Points += Cost;
            Player.Coins -= Cost;
            Cost = (int) (Cost + Mathf.Sqrt(Cost));
            Level++;
        }
    }
}