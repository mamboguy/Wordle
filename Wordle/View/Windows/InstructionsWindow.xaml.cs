using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wordle.View
{
    /// <summary>
    /// Interaction logic for InstructionsWindow.xaml
    /// </summary>
    public partial class InstructionsWindow
    {
        public InstructionsWindow(int guessCount, int wordLength)
        {
            InitializeComponent();

            instructionsText.Text = $"You know how wordle works.  You have { guessCount} " +
                                    $"guesses to find the answer.  The current word length " +
                                    $"is {wordLength}";

            this.
            ResizeMode = ResizeMode.NoResize;
            Width = 200;
            SizeToContent = SizeToContent.Height;
        }
    }
}
