using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wordle.Model.ColorStrategies;
using Wordle.Model.Settings;
using Wordle.Model.WordLists;
using Wordle.View;

namespace Wordle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const string ALPHABET = "QWERTYUIOPASDFGHJKL  ZXCVBNM  ";

        public IColorStrategy CurrentColors { get; private set; }
        private List<LetterBox> WordleBoxes { get; set; } = new List<LetterBox>();
        private List<LetterBox> KeyboardBoxes { get; set; } = new List<LetterBox>();
        private List<string> MasterWordList { get; init; }
        private int GuessCount { get; set; }
        private int WordLength { get; set; }
        private bool InstructionsOpened { get; set; }
        private bool SettingsOpened { get; set; }
        private string WordToGuess { get; set; }
        private int CurrentGuessRow { get; set; }
        private int CurrentGuessColumn { get; set; }
        private Random Random { get; init; }

        public MainWindow() : this(new StandardWordle(), new LightModeColorStrategy(), new SimpleWordList())
        {
        }

        public MainWindow(IGameSettings gameSettings, IColorStrategy colorStrategy, IWordList wordList)
        {
            InitializeComponent();

            GuessCount = gameSettings.GuessCount();
            WordLength = gameSettings.WordLength();
            CurrentColors = colorStrategy;
            MasterWordList = wordList.GetWordList();
            Random = new Random(DateTime.Now.Millisecond);

            DrawWindow();
        }

        #region UI
        public void DrawWindow()
        {
            ResetToDefault();

            CreateWordleGrid();
            CreateKeyboardGrid();

            RecolorWindow();

            SizeToContent = SizeToContent.Height;
            Width = 475;
            ResizeMode = ResizeMode.NoResize;
        }

        private void CreateKeyboardGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                var stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                };

                for (int j = 0; j < 10; j++)
                {
                    var box = new LetterBox(CurrentColors, i, j);
                    box.MouseDown += (s, e) => NonReservedKeyPressed(box.Character.ToString());
                    box.SetChar(ALPHABET[(i * 10) + j]);

                    KeyboardBoxes.Add(box);
                    stackPanel.Children.Add(box);
                }

                keyboardPanel.Children.Add(stackPanel);
            }
        }

        private void ResetToDefault()
        {
            wordlePanel.Children.Clear();
            keyboardPanel.Children.Clear();
            WordleBoxes.Clear();
            KeyboardBoxes.Clear();

            WordToGuess = MasterWordList[Random.Next(0, MasterWordList.Count - 1)].ToUpper();

            CurrentGuessColumn = 0;
            CurrentGuessRow = 0;
        }

        private void CreateWordleGrid()
        {
            for (int i = 0; i < GuessCount; i++)
            {
                var stackPanel = new StackPanel()
                {
                    Orientation = Orientation.Horizontal,
                };

                for (int j = 0; j < WordLength; j++)
                {
                    var newLetterBox = new LetterBox(CurrentColors, i, j);
                    WordleBoxes.Add(newLetterBox);
                    stackPanel.Children.Add(newLetterBox);
                }

                wordlePanel.Children.Add(stackPanel);
            }
        }

        public void RecolorWindow()
        {
            Background = new SolidColorBrush(CurrentColors.GetDefaultBackgroundColor());
            Foreground = new SolidColorBrush(CurrentColors.GetTextColor());

            WordleBoxes.ForEach(x =>
            {
                x.SetColors(CurrentColors);
            });

            KeyboardBoxes.ForEach(x =>
            {
                x.SetColors(CurrentColors);
            });

            WindowTitleBrush = new SolidColorBrush(CurrentColors.GetTitleBarColor());

            var textColor = CurrentColors.GetTextColor();
            enterButton.Foreground = new SolidColorBrush(textColor);
            instructionsButton.Foreground = new SolidColorBrush(textColor);
            newButton.Foreground = new SolidColorBrush(textColor);
            settingsButton.Foreground = new SolidColorBrush(textColor);
        }

        public void SetNewColorStrategy(IColorStrategy colorStrategy)
        {
            CurrentColors = colorStrategy;
            RecolorWindow();
        }

        private void CreatePopup(string message)
        {
            Window window = new Window()
            {
                Content = new TextBlock()
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                },
                Width = 200,                    
                SizeToContent = SizeToContent.Height,
            };

            window.Show();
        }
        #endregion

        #region Letterbox Finders
        private LetterBox GetKeyboardBox(char boxToColor)
        {
            for (int i = 0; i < KeyboardBoxes.Count; i++)
            {

                if (KeyboardBoxes[i].Character == boxToColor)
                {
                    return KeyboardBoxes[i];
                }
            }

            throw new Exception();
        }

        private LetterBox GetCurrentWordleBox()
        {
            return GetWordleBox(CurrentGuessRow, CurrentGuessColumn);
        }

        private LetterBox GetWordleBox(int row, int column)
        {
            return WordleBoxes.Where(x => x.ColIndex == column)
                        .Where(x => x.RowIndex == row)
                        .Single();
        }
        #endregion

        #region Letter Coloring
        private void ColorKeyboardBoxCorrect(char boxToColor)
        {
            GetKeyboardBox(boxToColor).SetBackgroundColorCorrect();
        }

        private void ColorKeyboardBoxApproximatelyCorrect(char boxToColor)
        {
            GetKeyboardBox(boxToColor).SetBackgroundColorApproximatelyCorrect();
        }

        private void ColorKeyboardBoxIncorrect(char boxToColor)
        {
            GetKeyboardBox(boxToColor).SetBackgroundColorIncorrect();
        }

        private void ColorAmberboxes(string guess)
        {
            var uniqueChars = guess.Distinct().ToList();

            foreach (var uniqueChar in uniqueChars)
            {
                int timesAppearsInMasterWord = WordToGuess.Where(x => x.Equals(uniqueChar))
                                                          .Count();

                int alreadyGreened = 0;

                for (int j = 0; j < WordLength; j++)
                {
                    if (WordToGuess[j] == uniqueChar && GuessAndMasterMatchAtIndex(guess, j))
                    {
                        alreadyGreened++;
                    }
                }

                // Handle duplicates in word
                // Duplicates should be handled such that if a guess is "WHERE" and the word is "THOSE"
                // The first E should not be colored and the second one should be green
                if (timesAppearsInMasterWord > 1)
                {
                    var index = 0;

                    while (alreadyGreened < timesAppearsInMasterWord)
                    {
                        if (guess[index] == uniqueChar && !GuessAndMasterMatchAtIndex(guess, index))
                        {
                            GetWordleBox(CurrentGuessRow, index).SetBackgroundColorApproximatelyCorrect();
                            ColorKeyboardBoxApproximatelyCorrect(guess[index]);
                            alreadyGreened++;
                        }
                    }
                }
                else if (timesAppearsInMasterWord == 1 && alreadyGreened == 0)
                {
                    int index = guess.IndexOf(uniqueChar);
                    GetWordleBox(CurrentGuessRow, index).SetBackgroundColorApproximatelyCorrect();
                    ColorKeyboardBoxApproximatelyCorrect(guess[index]);
                }
                else
                {
                    // Do nothing
                    ColorKeyboardBoxIncorrect(uniqueChar);
                }
            }
        }

        private void ColorGreenBoxes(string guess)
        {
            for (int i = 0; i < WordLength; i++)
            {
                if (GuessAndMasterMatchAtIndex(guess, i))
                {
                    GetWordleBox(CurrentGuessRow, i).SetBackgroundColorCorrect();
                    ColorKeyboardBoxCorrect(guess[i]);
                }
            }
        }
        #endregion

        #region Program Logic
        private void CheckGuess()
        {
            var guess = GetGuessFromRow();

            ColorGreenBoxes(guess);
            ColorAmberboxes(guess);

            CurrentGuessRow++;
            CurrentGuessColumn = 0;

            if (guess != WordToGuess)
            {
                if (CurrentGuessRow >= GuessCount)
                {
                    CreatePopup($"I'm sorry, you ran out of guesses.  The correct word was {WordToGuess}");
                }
            }
            else
            {
                CreatePopup($"CONGRATS!  You won in {CurrentGuessRow} guesses");
            }
        }

        private bool GuessAndMasterMatchAtIndex(string guess, int i)
        {
            return guess[i].Equals(WordToGuess[i]);
        }

        private string GetGuessFromRow()
        {
            string guess = "";

            for (int i = 0; i < WordLength; i++)
            {
                guess += GetWordleBox(CurrentGuessRow, i).Character;
            }

            return guess.Trim();
        }

        private bool IsValidInput(string keyPressed)
        {
            return Regex.Match(keyPressed, "[a-zA-Z]").Success;
        }
        #endregion

        #region Event Handlers
        private void InstructionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!InstructionsOpened)
            {
                // Create a frame of the new window
                var instructionsPopup = new InstructionsWindow(GuessCount, WordLength);

                // When the window closes, reset the InstructionsOpened flag
                instructionsPopup.Closing += (s, e) =>
                {
                    InstructionsOpened = false;
                };

                // Show the window and set the InstructionsOpened flag
                instructionsPopup.Show();
                InstructionsOpened = true;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SettingsOpened)
            {
                // Create a frame of the new window
                var settingsPopup = new SettingsWindow(this);

                // When the window closes, reset the InstructionsOpened flag
                settingsPopup.Closing += (s, e) =>
                {
                    SettingsOpened = false;
                };

                // Show the window and set the InstructionsOpened flag
                settingsPopup.Show();
                SettingsOpened = true;
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            DrawWindow();
        }

        public void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    e.Handled = true; // Prevent system ping

                    EnterKeyPressed(null, null);
                    break;
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Back:
                    if (CurrentGuessColumn != 0)
                    {
                        CurrentGuessColumn--;
                    }

                    GetCurrentWordleBox().SetChar(' ');
                    break;
                default:
                    NonReservedKeyPressed(e.Key.ToString());
                    break;
            }
        }

        public void EnterKeyPressed(object sender, RoutedEventArgs e)
        {
            if (CurrentGuessColumn == WordLength)
            {
                CheckGuess();
            }
        }

        private void NonReservedKeyPressed(string keyPress)
        {
            if (IsValidInput(keyPress))
            {
                if (CurrentGuessColumn < WordLength)
                {
                    GetCurrentWordleBox().SetChar(keyPress.ToUpper()[0]);

                    CurrentGuessColumn++;
                }
            }
        }
        #endregion
    }
}
