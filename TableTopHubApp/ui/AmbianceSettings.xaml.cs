using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    public struct AmbianceData
    {
        public string Id;

        public bool Looping;
        public bool Fluctuating;

        public double Volume;
        public double Frequency;
        public double Variance;
    }

    /// <summary>
    /// Interaction logic for AmbianceSettings.xaml
    /// </summary>
    public partial class AmbianceSettings : UserControl
    {
        public AmbianceSettings()
        {
            InitializeComponent();

            List<ManifestEntry> allSfx = AudioManager.GetTracks();
            allSfx.AddRange(AudioManager.GetSoundEffects());
            
            this.audioOptions.ItemsSource = allSfx;
            this.audioOptions.DisplayMemberPath = "Name";
            this.audioOptions.SelectedValuePath = "Id";
            this.audioOptions.SelectedValue = 0;
        }

        public AmbianceData ReadData()
        {
            AmbianceData data = new AmbianceData();

            data.Looping = (bool)this.loopingCheck.IsChecked;
            data.Fluctuating = (bool)this.pitchCheck.IsChecked;

            data.Id = (string)this.audioOptions.SelectedValue;

            data.Frequency = this.frequencySlider.Value;
            data.Volume = this.volumeSlider.Value;
            data.Variance = this.variationSlider.Value;

            return data;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(this) as StackPanel;

            parent?.Children.Remove(this);
        }
    }
}
