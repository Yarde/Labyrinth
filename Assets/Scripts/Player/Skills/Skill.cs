using UnityEngine;

namespace Skills
{
    public abstract class Skill
    {
        protected Player _player;
        public SkillData Data;
        public int Level;

        protected Skill(Player player, SkillData data)
        {
            _player = player;
            Data = data;
        }
        
        public virtual void Upgrade()
        {
            if (_player.Coins < Data.cost)
            {
                Debug.Log($"Not enough coins, player has {_player.Coins} but {Data.cost} is required for skill {Data.name}");
                return;
            }
            
            if (Level == Data.maxLevel)
            {
                Debug.Log($"Already max level {Data.maxLevel} for skill {Data.name}");
                return;
            }

            _player.Coins -= Data.cost;
            Data.cost = (int) (Data.cost + Mathf.Sqrt(Data.cost));
            Level++;
        }
    }
}