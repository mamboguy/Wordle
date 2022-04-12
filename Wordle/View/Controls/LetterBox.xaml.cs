using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wordle.Model.ColorStrategies;

namespace Wordle.View
{
    /// <summary>
    /// Interaction logic for LetterBox.xaml
    /// </summary>
    public partial class LetterBox : UserControl
    {
        public LetterBox() : this(new LightModeColorStrategy(), 0, 0)
        {

        }

        public int RowIndex { get; init; }
        public int ColIndex { get; init; }

        public LetterBox(IColorStrategy boxColors, int rowIndex, int colIndex)
        {
            InitializeComponent();

            this.Height = 40;
            this.Width = 40;

            RowIndex = rowIndex;
            ColIndex = colIndex;

            SetColors(boxColors);

            // TODO - Sticky colors for guessed words
            CanOverrideColoring = true;
        }

        public void SetColors(IColorStrategy boxColors)
        {
            if (CanOverrideColoring)
            {
                Background = MakeSolidBrush(boxColors.GetDefaultBackgroundColor());

            }
            else if (IsIncorrectColor)
            {
                Background = MakeSolidBrush(boxColors.GetIncorrectBackgroundColor());
            }

            if (Character != ' ')
            {
                BorderBrush = MakeSolidBrush(boxColors.GetBorderColor());
                BorderThickness = new Thickness(2);
            }

            charLabel.Foreground = MakeSolidBrush(boxColors.GetTextColor());

            CorrectColor = boxColors.GetCorrectBackgroundColor();
            IncorrectColor = boxColors.GetIncorrectBackgroundColor();
            ApproximatelyCorrectColor = boxColors.GetApproximatelyCorrectBackgroundColor();
        }

        private Color CorrectColor { get; set; }
        private Color IncorrectColor { get; set; }
        private Color ApproximatelyCorrectColor { get; set; }
        private bool CanOverrideColoring { get; set; }
        private bool IsIncorrectColor { get; set; }
        public char Character { get; private set; }
        public Key Key { get; internal set; }

        private Brush MakeSolidBrush(Color color)
        {
            return new SolidColorBrush(color);
        }

        public void SetBackgroundColorIncorrect()
        {
            if (CanOverrideColoring)
            {
                Background = MakeSolidBrush(IncorrectColor);
                CanOverrideColoring = false;
                IsIncorrectColor = true;
            }
        }

        public void SetBackgroundColorCorrect()
        {
            if (CanOverrideColoring)
            {
                Background = MakeSolidBrush(CorrectColor);
                CanOverrideColoring = false;
            }
        }

        public void SetBackgroundColorApproximatelyCorrect()
        {
            if (CanOverrideColoring)
            {
                Background = MakeSolidBrush(ApproximatelyCorrectColor);
                CanOverrideColoring = false;
            }
        }

        public void SetChar(char boxChar)
        {
            Character = boxChar;
            charLabel.Content = boxChar;
        }

        public void Reset()
        {
            Character = ' ';
        }
    }
}
