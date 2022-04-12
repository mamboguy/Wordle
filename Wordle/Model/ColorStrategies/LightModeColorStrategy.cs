using System.Windows.Media;

namespace Wordle.Model.ColorStrategies
{
    public class LightModeColorStrategy : IColorStrategy
    {
        public Color GetDefaultBackgroundColor()
        {
            return Color.FromRgb(255, 255, 255); // White
        }

        public Color GetBorderColor()
        {
            return Color.FromRgb(200, 200, 200);
        }

        public Color GetTextColor()
        {
            return Color.FromRgb(0, 0, 0);
        }

        public Color GetCorrectBackgroundColor()
        {
            return Color.FromRgb(0, 128, 0);
        }

        public Color GetIncorrectBackgroundColor()
        {
            return Color.FromRgb(200, 200, 200);
        }

        public Color GetApproximatelyCorrectBackgroundColor()
        {
            return Color.FromRgb(218, 165, 32);
        }

        public Color GetTitleBarColor()
        {
            return Color.FromRgb(102, 178, 255);
        }
    }
}
