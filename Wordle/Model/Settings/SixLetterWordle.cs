namespace Wordle.Model.Settings
{
    public class SixLetterWordle : IGameSettings
    {
        public int GuessCount()
        {
            return 7;
        }

        public int WordLength()
        {
            return 6;
        }
    }
}
