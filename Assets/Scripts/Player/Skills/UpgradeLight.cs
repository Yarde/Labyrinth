namespace Player.Skills
{
    class UpgradeLight : Skill
    {
        public UpgradeLight(Player player, SkillData data) : base(player, data)
        {
        }
        
        public override void Upgrade()
        {
            base.Upgrade();
            Player.LightLevel += Data.bonusPerLevel;
        }
    }
}