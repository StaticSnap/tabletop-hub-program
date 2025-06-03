// <copyright file="MusicScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The music screen contains UI elements to interact with the other two screens of the program
    /// it contains options for music, overlay and battlemap.
    /// </summary>
    public partial class MusicScreen : Window
    {
        private Grid activeGrid = new Grid();
        private Grid activeSubGrid = new Grid();
        private Grid activeSubSubGrid = new Grid();

        private App parentRef;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicScreen"/> class.
        /// </summary>
        /// <param name="appRef">Reference to the main thread.</param>
        public MusicScreen(App appRef)
        {
            this.InitializeComponent();

            // makes sure that all resources exist
            FileManager.EnsurePathways();

            MapManager.InitMaps();
            AudioManager.InitTracks();
            OverlayManager.InitAssets();
            AudioPlayer audioPlayer = new AudioPlayer();

            this.musicOptions.ItemsSource = AudioManager.GetTracks();
            this.musicOptions.DisplayMemberPath = "Name";
            this.musicOptions.SelectedValuePath = "Id";
            this.musicOptions.SelectedIndex = 0;

            this.soundOptions.ItemsSource = AudioManager.GetSoundEffects();
            this.soundOptions.DisplayMemberPath = "Name";
            this.soundOptions.SelectedValuePath = "Id";
            this.soundOptions.SelectedIndex = 0;

            this.overlayOptions.ItemsSource = OverlayManager.GetOverlayTitles();
            this.overlayOptions.SelectedIndex = 0;

            this.iconOptions.ItemsSource = MapManager.GetIconTitles();
            this.iconOptions.SelectedIndex = 0;

            this.mapOptions.ItemsSource = MapManager.GetMaps();
            this.mapOptions.DisplayMemberPath = "Name";
            this.mapOptions.SelectedValuePath = "Id";
            this.mapOptions.SelectedIndex = 0;

            this.activeGrid = this.mainGrid;

            this.parentRef = appRef;
        }

        /// <summary>
        /// Event handler for when the main tab of UI is changed.
        /// </summary>
        /// <exception cref="Exception">Exception thrown when event is flagged by a non-button.</exception>
        private void ScreenChangeClick(object sender, RoutedEventArgs e)
        {
            // Ensure that screen is properly reset.
            this.ContentClearAll();

            // Get data on button that flagged event.
            Button sourceButton = new Button();
            if (sender.GetType() == typeof(Button)) 
            {
                sourceButton = (Button)sender;
            }
            else
            {
                throw new Exception("Event not raised by button");
            }

            // Hide the current screen.
            this.activeGrid.IsEnabled = false;
            this.activeGrid.Visibility = Visibility.Hidden;

            // Depending on the sender's name, enable correct screen.
            if (sourceButton.Name == "mainScreenButton")
            {
                this.mainGrid.IsEnabled = true;
                this.mainGrid.Visibility = Visibility.Visible;

                this.activeGrid = this.mainGrid;
            }
            else if(sourceButton.Name == "addContentScreenButton")
            {
                this.addContentGrid.IsEnabled = true;
                this.addContentGrid.Visibility = Visibility.Visible;

                this.activeGrid = this.addContentGrid;
            }
            else if(sourceButton.Name == "statsScreenButton")
            {
                this.statsGrid.IsEnabled = true;
                this.statsGrid.Visibility = Visibility.Visible;

                this.activeGrid = this.statsGrid;
            }
        }

        /// <summary>
        /// Event handler for when the type of content to add is changed.
        /// </summary>
        /// <exception cref="Exception">Exception thrown when event is flagged by a non-button.</exception>
        private void AddContentSubScreenChange(object sender, RoutedEventArgs e)
        {
            // Ensure content screen is properly cleared.
            this.ContentClearAll();

            // Get data on sending button.
            Button sourceButton = new Button();
            if (sender.GetType() == typeof(Button))
            {
                sourceButton = (Button)sender;
            }
            else
            {
                throw new Exception("Event not raised by button");
            }

            // Disable old grid.
            this.activeSubGrid.IsEnabled = false;
            this.activeSubGrid.Visibility = Visibility.Hidden;

            // Enable new screen based off button info.
            if(sourceButton.Name == "addContentButton")
            {
                this.addContentSubGrid.IsEnabled = true;
                this.addContentSubGrid.Visibility = Visibility.Visible;

                this.activeSubGrid = this.addContentSubGrid;
            }
            else if(sourceButton.Name == "editContentButton")
            {
                this.editContentSubGrid.IsEnabled = true;
                this.editContentSubGrid.Visibility = Visibility.Visible;

                this.activeSubGrid = this.editContentSubGrid;
            }
            else if(sourceButton.Name == "removeContentButton")
            {
                this.removeContentSubGrid.IsEnabled = true;
                this.removeContentSubGrid.Visibility = Visibility.Visible;

                this.activeSubGrid = this.removeContentSubGrid;
            }
        }

        private void AddContentTypeDropdownChanged(object sender, RoutedEventArgs e)
        {
            string selected = this.addContentTypeDropdown.SelectedValue.ToString();

            this.activeSubSubGrid.IsEnabled = false;
            this.activeSubSubGrid.Visibility = Visibility.Hidden;

            this.ContentClearAll();

            if (selected == "System.Windows.Controls.ComboBoxItem: music")
            {
                this.addContentMusicGrid.IsEnabled = true;
                this.addContentMusicGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentMusicGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: sound")
            {
                this.addContentSoundGrid.IsEnabled = true;
                this.addContentSoundGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentSoundGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: overlay")
            {
                this.addContentOverlayGrid.IsEnabled = true;
                this.addContentOverlayGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentOverlayGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: icon")
            {
                this.addContentIconGrid.IsEnabled = true;
                this.addContentIconGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentIconGrid;
            }
            else if (selected == "System.Windows.Controls.ComboBoxItem: map")
            {
                this.addContentMapGrid.IsEnabled = true;
                this.addContentMapGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentMapGrid;
            }
            else if(selected == null)
            {
                // do nothing
            }
            else
            {
                throw new Exception("type not implemented");
            }
        }

        private void ContentClearAll()
        {
            FileManager.ClearBuffers();

            // add section
            this.addContentMusicIntroOpenFileFeedback.Text = string.Empty;
            this.addContentMusicOpenFileFeedback.Text = string.Empty;
            this.addContentMusicNameBox.Text = string.Empty;
            this.addContentMusicConfirmFeedback.Text = string.Empty;

            this.addContentSoundOpenFileFeedback.Text = string.Empty;
            this.addContentSoundNameBox.Text = string.Empty;
            this.addContentSoundConfirmFeedback.Text = string.Empty;

            this.addContentIconOpenFileFeedback.Text = string.Empty;
            this.addContentIconNameBox.Text = string.Empty;
            this.AddContentIconWidthBox.Text = string.Empty;
            this.AddContentIconHeightBox.Text = string.Empty;
            this.addContentIconConfirmFeedback.Text = string.Empty;

            this.addContentOverlayOpenFileFeedback.Text = string.Empty;
            this.addContentOverlayNameBox.Text = string.Empty;
            this.addContentOverlayConfirmFeedback.Text = string.Empty;

            this.addContentMapOpenFileFeedback.Text = string.Empty;
            this.addContentMapNameBox.Text = string.Empty;
            this.AddContentMapWidthBox.Text = string.Empty;
            this.AddContentMapHeightBox.Text = string.Empty;
            this.addContentMapConfirmFeedback.Text = string.Empty;

            // edit section

            // remove section
        }

        private void AddContentOpenFileClick(object sender, RoutedEventArgs e)
        {
            Button sourceButton = new Button();
            if (sender.GetType() == typeof(Button))
            {
                sourceButton = (Button)sender;
            }
            else
            {
                throw new Exception("Event not raised by button");
            }

            bool result = false;

            if(sourceButton.Name == "addContentMusicOpenFileButton")
            {
                result = FileManager.OpenFile("music");
                if (result == true)
                {
                    this.addContentMusicOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentMusicOpenFileFeedback.Text = "Error: could not open file";
                }

                return;
            }
            else if (sourceButton.Name == "addContentMusicIntroOpenFileButton")
            {
                result = FileManager.OpenIntroFile();
                if (result == true)
                {
                    this.addContentMusicIntroOpenFileFeedback.Text = "Current file: " + FileManager.CurrentIntroPath;
                }
                else
                {
                    this.addContentMusicIntroOpenFileFeedback.Text = "Error: could not open file";
                }

                return;
            }
            else if(sourceButton.Name == "addContentSoundOpenFileButton")
            {
                result = FileManager.OpenFile("sound");
                if (result == true)
                {
                    this.addContentSoundOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentSoundOpenFileFeedback.Text = "Error: could not open file";
                }

                return;
            }
            else if(sourceButton.Name == "addContentOverlayOpenFileButton")
            {
                result = FileManager.OpenFile("overlay");
                if (result == true)
                {
                    this.addContentOverlayOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentOverlayOpenFileFeedback.Text = "Error: could not open file";
                }

                return;
            }
            else if(sourceButton.Name == "addContentMapOpenFileButton")
            {
                result = FileManager.OpenFile("map");
                if (result == true)
                {
                    this.addContentMapOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentMapOpenFileFeedback.Text = "Error: could not open file";
                }
            }
            else if(sourceButton.Name == "addContentIconOpenFileButton")
            {
                result = FileManager.OpenFile("icon");
                if (result == true)
                {
                    this.addContentIconOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                }
                else
                {
                    this.addContentIconOpenFileFeedback.Text = "Error: could not open file";
                }
            }
            else
            {
                throw new Exception("unimplemented resource type");
            }
        }

        private void AddContentMusicConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[3];

            if (this.addContentMusicNameBox.Text.Length > 0)
            {
                data[0] = this.addContentMusicNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentMusicConfirmFeedback.Text = "Error: no track name provided";
                return;
            }

            if(FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentMusicConfirmFeedback.Text = "Error: no loop file selected";
                return;
            }

            if(FileManager.CurrentIntroPath != string.Empty)
            {
                data[2] = Path.GetFileName(FileManager.CurrentIntroPath);
            }
            else
            {
                data[2] = "NULL";
            }

            FileManager.CopyFile("music");
            if (data[2] != "NULL")
            {
                FileManager.CopyFile("intro");
            }

            FileManager.AddData("music", data);
            this.ContentClearAll();
            this.addContentMusicConfirmFeedback.Text = "Successfully added!";
            AudioManager.InitTracks();
            this.musicOptions.ItemsSource = AudioManager.GetTracks();
            this.musicOptions.DisplayMemberPath = "Name";
            this.musicOptions.SelectedValuePath = "Id";
            this.musicOptions.SelectedIndex = 0;
        }

        private void AddContentSoundConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[2];

            if(this.addContentSoundNameBox.Text.Length > 0)
            {
                data[0] = this.addContentSoundNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentSoundConfirmFeedback.Text = "Error: no sound name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentSoundConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            FileManager.CopyFile("sound");

            FileManager.AddData("sound", data);
            this.ContentClearAll();
            this.addContentSoundConfirmFeedback.Text = "Successfully added!";
            AudioManager.InitTracks();

            this.soundOptions.ItemsSource = StorageManager.GetAllSoundEntries();
            this.soundOptions.DisplayMemberPath = "Name";
            this.soundOptions.SelectedValuePath = "Id";
            this.soundOptions.SelectedIndex = 0;
            this.soundOptions.SelectedIndex = 0;
        }

        private void AddContentIconConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[5];

            if (this.addContentIconNameBox.Text.Length > 0)
            {
                data[0] = this.addContentIconNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            if(Path.GetExtension(FileManager.CurrentFilePath) == ".gif")
            {
                data[2] = "ANIMATED";
            }
            else
            {
                data[2] = "STATIC";
            }

            int width = 1;
            if (this.AddContentIconWidthBox.Text.Length > 0)
            {
                if (int.TryParse(this.AddContentIconWidthBox.Text, out width))
                {
                    data[3] = System.Text.RegularExpressions.Regex.Replace(this.AddContentIconWidthBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.addContentIconConfirmFeedback.Text = "Error: please use only numbers in your width, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no width provided";
                return;
            }

            int height = 1;
            if (this.AddContentIconHeightBox.Text.Length > 0)
            {
                if (int.TryParse(this.AddContentIconHeightBox.Text, out height))
                {
                    data[4] = System.Text.RegularExpressions.Regex.Replace(this.AddContentIconHeightBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.addContentIconConfirmFeedback.Text = "Error: please use only numbers in your height, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.addContentIconConfirmFeedback.Text = "Error: no height provided";
                return;
            }

            FileManager.CopyFile("icon");
            FileManager.AddData("icon", data);
            this.ContentClearAll();
            this.addContentIconConfirmFeedback.Text = "Successfully added!";
            MapManager.InitMaps();
            this.iconOptions.ItemsSource = MapManager.GetIconTitles();
            this.iconOptions.SelectedIndex = 0;
        }

        private void AddContentOverlayConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[5];
            if (this.addContentOverlayNameBox.Text.Length > 0)
            {
                data[0] = this.addContentOverlayNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentOverlayConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentOverlayConfirmFeedback.Text = "Error: no name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentOverlayConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            if (Path.GetExtension(FileManager.CurrentFilePath) == ".mov" || Path.GetExtension(FileManager.CurrentFilePath) == ".mp4")
            {
                data[2] = "VIDEO";
                data[3] = "FALSE";
                data[4] = "NULL";
            }
            else
            {
                data[2] = "IMAGE";
                data[3] = "NULL";
                data[4] = "NULL";
            }

            FileManager.CopyFile("overlay");
            FileManager.AddData("overlay", data);
            this.ContentClearAll();
            this.addContentOverlayConfirmFeedback.Text = "Successfully added!";
            OverlayManager.InitAssets();
            this.overlayOptions.ItemsSource = OverlayManager.GetOverlayTitles();
            this.overlayOptions.SelectedIndex = 0;
        }

        private void AddContentMapConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[4];

            if (this.addContentMapNameBox.Text.Length > 0)
            {
                data[0] = this.addContentMapNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentMapConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.addContentMapConfirmFeedback.Text = "Error: no name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.addContentMapConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            int width = 1;
            if (this.AddContentMapWidthBox.Text.Length > 0)
            {
                if (int.TryParse(this.AddContentMapWidthBox.Text, out width))
                {
                    if(width > 400)
                    {
                        this.addContentMapConfirmFeedback.Text = "Error: please use a width of 400 tiles or lower";
                        return;
                    }
                    else
                    {
                        data[2] = System.Text.RegularExpressions.Regex.Replace(this.AddContentMapWidthBox.Text, @"\s", string.Empty);
                    }
                }
                else
                {
                    this.addContentMapConfirmFeedback.Text = "Error: please use only numbers in your width, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.addContentMapConfirmFeedback.Text = "Error: no width provided";
                return;
            }

            int height = 1;
            if (this.AddContentMapHeightBox.Text.Length > 0)
            {
                if (int.TryParse(this.AddContentMapHeightBox.Text, out height))
                {
                    if(height > 400)
                    {
                        this.addContentMapConfirmFeedback.Text = "Error: please use a height of 400 tiles or lower";
                        return;
                    }
                    else
                    {
                        data[3] = System.Text.RegularExpressions.Regex.Replace(this.AddContentMapHeightBox.Text, @"\s", string.Empty);
                    }
                }
                else
                {
                    this.addContentMapConfirmFeedback.Text = "Error: please use only numbers in your height, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.addContentMapConfirmFeedback.Text = "Error: no height provided";
                return;
            }

            FileManager.CopyFile("map");
            FileManager.AddData("map", data);
            this.ContentClearAll();
            this.addContentMapConfirmFeedback.Text = "Successfully added!";
            MapManager.InitMaps();
            this.mapOptions.ItemsSource = MapManager.GetMaps();
            this.mapOptions.DisplayMemberPath = "Name";
            this.mapOptions.SelectedValuePath = "Id";
            this.mapOptions.SelectedIndex = 0;
        }

        private void PlayMusicClick(object sender, RoutedEventArgs e)
        {
            if (this.musicOptions.SelectedValue != null)
            {
                AudioPlayer.PrepMusicWorker(this.musicOptions.SelectedValue.ToString());
            }
        }

        private void StopMusicClick(object sender, RoutedEventArgs e)
        {
            AudioPlayer.StopTrack();
        }

        private void PlaySoundClick(object sender, RoutedEventArgs e)
        {
            if (this.soundOptions.SelectedValue != null)
            {
                AudioPlayer.PrepSoundEffectWorker(this.soundOptions.SelectedValue.ToString() !);
            }
        }

        private void StopSoundClick(object sender, RoutedEventArgs e)
        {
            // todo
        }

        private void EnableOverlayClick(object sender, RoutedEventArgs e)
        {
            if (this.overlayOptions.SelectedValue != null && App.OverlayRunning)
            {
                App.OverlayTab.EnableOverlayElement(this.overlayOptions.SelectedValue.ToString() !);
            }
        }

        private void DisableOverlayClick(object sender, RoutedEventArgs e)
        {
            if (App.OverlayRunning){
                App.OverlayTab.DisableOverlayElement();
            }
        }

        private void EditOverlayClick(object sender, RoutedEventArgs e)
        {
            if (App.OverlayRunning)
            {
                App.OverlayTab.ChangeWindowState();
            }
        }

        private void AddIconClick(object sender, RoutedEventArgs e)
        {
            if (this.iconOptions.SelectedValue != null && App.BattleRunning)
            {
                App.BattleTab.AddCreature(this.iconOptions.SelectedValue.ToString()!);
            }
        }

        private void SelectMapClick(object sender, RoutedEventArgs e)
        {
            if (this.mapOptions.SelectedValue != null && App.BattleRunning)
            {
                App.BattleTab.OpenMap(this.mapOptions.SelectedValue.ToString()!);
            }
        }

        /// <summary>
        /// Event raised when the window is closed. makes sure to end playback of music. in the future close the other windows when this happens.
        /// </summary>
        private void MusicScreenClosing(object sender, CancelEventArgs e)
        {
            AudioPlayer.StopTrack();

            this.parentRef.BattleThreadKill();
            this.parentRef.OverlayThreadKill();
            this.parentRef.EndApp();
        }

        private void MusicVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioPlayer.ChangeMusicVolume((int)this.musicVolume.Value);
        }

        private void SoundVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            AudioPlayer.ChangeSoundEffectVolume((int)this.soundVolume.Value);
        }
    }
}
