namespace UI
{
    public class UpgradeMovement : Skill
    {
        public UpgradeMovement(Player player, SkillData data) : base(player, data)
        {
        }

        public override void Upgrade()
        {
            base.Upgrade();
            _player.MovementSpeed += Data.BonusPerLevel;
        }
    }
}