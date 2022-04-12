using System.Windows.Media;

namespace Wordle.Model.ColorStrategies
{
    public interface IColorStrategy
    {
        public Color GetDefaultBackgroundColor();
        public Color GetTextColor();
        public Color GetBorderColor();
        public Color GetCorrectBackgroundColor();
        public Color GetIncorrectBackgroundColor();
        public Color GetApproximatelyCorrectBackgroundColor();
        public Color GetTitleBarColor();
    }
}