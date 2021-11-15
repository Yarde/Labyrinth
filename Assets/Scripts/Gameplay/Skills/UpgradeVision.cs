namespace Gameplay.Skills
{
    public class UpgradeVision : Skill
    {
        public UpgradeVision(Player player, SkillData data) : base(player, data)
        {
        }

        public override void Upgrade()
        {
            base.Upgrade();
            Player.FieldOfViewLevel += Data.bonusPerLevel;
        }
    }
}