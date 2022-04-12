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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wordle.Model.ColorStrategies;

namespace Wordle.View.Controls
{
    /// <summary>
    /// Interaction logic for Keyboard.xaml
    /// </summary>
    public partial class Keyboard : UserControl
    {
        private const string ALPHABET = "QWERTYUIOPASDFGHJKL  ZXCVBNM  ";

        private MainWindow Parent { get; }

        public Keyboard() : this(new LightModeColorStrategy(), null)
        {            
        }

        public Keyboard(IColorStrategy colorStrategy, MainWindow parent)
        {
            InitializeComponent();

            Parent = parent;
        }

        
    }
}
