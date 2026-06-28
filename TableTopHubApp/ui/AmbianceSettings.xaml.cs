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

namespace TableTopHubApp.ui
{
    /// <summary>
    /// Interaction logic for AmbianceSettings.xaml
    /// </summary>
    public partial class AmbianceSettings : UserControl
    {
        public AmbianceSettings()
        {
            InitializeComponent();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this) as StackPanel;

            parent?.Children.Remove(this);
        }
    }
}
