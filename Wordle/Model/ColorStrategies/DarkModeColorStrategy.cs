using System.Windows.Media;

namespace Wordle.Model.ColorStrategies
{
    public class DarkModeColorStrategy : IColorStrategy
    {
        public Color GetDefaultBackgroundColor()
        {
            return Color.FromRgb(32, 32, 32);
        }

        public Color GetBorderColor()
        {
            return Color.FromRgb(128, 128, 128);
        }

        public Color GetTextColor()
        {
            return Color.FromRgb(255, 255, 255);
        }

        public Color GetCorrectBackgroundColor()
        {
            return Color.FromRgb(0, 128, 0);
        }

        public Color GetIncorrectBackgroundColor()
        {
            return Color.FromRgb(128, 128, 128);
        }

        public Color GetApproximatelyCorrectBackgroundColor()
        {
            return Color.FromRgb(218, 165, 32);
        }

        public Color GetTitleBarColor()
        {
            return Color.FromRgb(32, 32, 32);
        }
    }
}
