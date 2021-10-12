namespace Skills
{
    class UpgradeLight : Skill
    {
        public UpgradeLight(Player player, SkillData data) : base(player, data)
        {
        }
        
        public override void Upgrade()
        {
            base.Upgrade();
            _player.LightLevel += Data.bonusPerLevel;
        }
    }
}