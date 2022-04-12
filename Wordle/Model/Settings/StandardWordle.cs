namespace Wordle.Model.Settings
{
    public class StandardWordle : IGameSettings
    {
        public int GuessCount()
        {
            return 6;
        }

        public int WordLength()
        {
            return 5;
        }
    }
}
