namespace Player.Skills
{
    public class UpgradeMovement : Skill
    {
        public UpgradeMovement(Player player, SkillData data) : base(player, data)
        {
        }

        public override void Upgrade()
        {
            base.Upgrade();
            Player.MovementSpeed += Data.bonusPerLevel;
        }
    }
}