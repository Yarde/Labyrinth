namespace UI.Windows.Questions
{
    public class QuestionResult
    {
        public int Coins { get; set; }
        public int Experience { get; set; }
        public int Hearts { get; set; }
        public int Points { get; set; }

        public QuestionResult(int coins, int experience, int hearts, int points)
        {
            Coins = coins;
            Experience = experience;
            Hearts = hearts;
            Points = Points;
        }
    }
}