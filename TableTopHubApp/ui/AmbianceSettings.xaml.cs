namespace TableTopHubApp.ui
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for AmbianceSettings.xaml.
    /// </summary>
    public partial class AmbianceSettings : UserControl
    {
        public AmbianceSettings()
        {
            this.InitializeComponent();

            List<ManifestEntry> allSfx = new List<ManifestEntry>();
            allSfx.AddRange(AudioManager.GetTracks());
            allSfx.AddRange(AudioManager.GetSoundEffects());
            
            this.audioOptions.ItemsSource = allSfx;
            this.audioOptions.DisplayMemberPath = "Name";
            this.audioOptions.SelectedValuePath = "Id";
            this.audioOptions.SelectedValue = 0;
        }

        public SoundInfo ReadData()
        {
            SoundInfo data = new SoundInfo();

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

        private void LoopingClick(object sender, RoutedEventArgs e)
        {
            if((bool)this.loopingCheck.IsChecked)
            {
                this.frequencySlider.IsEnabled = false;
            }
            else
            {
                this.frequencySlider.IsEnabled = true;
            }
        }

        private void PitchClick(object sender, RoutedEventArgs e)
        {
            if((bool)this.pitchCheck.IsChecked)
            {
                this.variationSlider.IsEnabled = true;
            }
            else
            {
                this.variationSlider.IsEnabled = false;
            }
        }
    }
}
