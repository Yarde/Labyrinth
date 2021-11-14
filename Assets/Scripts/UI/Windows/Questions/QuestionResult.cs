namespace UI.Windows.Questions
{
    public class QuestionResult
    {
        public int Coins { get; set; }
        public int Experience { get; set; }
        public int Hearts { get; set; }

        public QuestionResult(int coins, int experience, int hearts)
        {
            Coins = coins;
            Experience = experience;
            Hearts = hearts;
        }
    }
}