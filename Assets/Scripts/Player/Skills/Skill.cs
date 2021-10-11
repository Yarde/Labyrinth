using UnityEngine;

namespace UI
{
    public abstract class Skill
    {
        public struct SkillData
        {
            public string SkillName;
            public int MaxLevel;
            public float BonusPerLevel;
            public int Cost;
            public int Level;
        }
        
        protected Player _player;
        public SkillData Data;

        protected Skill(Player player, SkillData data)
        {
            _player = player;
            Data = data;
        }
        
        public virtual void Upgrade()
        {
            if (_player.Coins < Data.Cost)
            {
                Debug.Log($"Not enough coins, player has {_player.Coins} but {Data.Cost} is required for skill {Data.SkillName}");
                return;
            }
            
            if (Data.Level == Data.MaxLevel)
            {
                Debug.Log($"Already max level {Data.MaxLevel} for skill {Data.SkillName}");
                return;
            }

            _player.Coins -= Data.Cost;
        }
    }
}