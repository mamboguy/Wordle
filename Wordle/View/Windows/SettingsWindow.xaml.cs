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
using Wordle.Model.ColorStrategies;

namespace Wordle.View
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        private MainWindow Parent { get; set; }

        public SettingsWindow(MainWindow parent)
        {
            InitializeComponent();
            Parent = parent;

            colorModeToggle.IsOn = parent.CurrentColors is DarkModeColorStrategy;

            ResizeMode = ResizeMode.NoResize;
            SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void ChangeColorMode(object sender, RoutedEventArgs e)
        {
            if (colorModeToggle.IsOn)
            {
                Parent.SetNewColorStrategy(new DarkModeColorStrategy());
            }
            else
            {
                Parent.SetNewColorStrategy(new LightModeColorStrategy());
            }

            Parent.RecolorWindow();
        }
    }
}
