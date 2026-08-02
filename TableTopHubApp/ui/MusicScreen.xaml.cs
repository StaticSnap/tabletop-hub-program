// <copyright file="MusicScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using TableTopHubApp.ui;

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

            this.overlayOptions.ItemsSource = OverlayManager.GetOverlays();
            this.overlayOptions.DisplayMemberPath = "Name";
            this.overlayOptions.SelectedValuePath = "Id";
            this.overlayOptions.SelectedIndex = 0;

            this.iconOptions.ItemsSource = MapManager.GetIcons();
            this.iconOptions.DisplayMemberPath = "Name";
            this.iconOptions.SelectedValuePath = "Id";
            this.iconOptions.SelectedIndex = 0;

            this.mapOptions.ItemsSource = MapManager.GetMaps();
            this.mapOptions.DisplayMemberPath = "Name";
            this.mapOptions.SelectedValuePath = "Id";
            this.mapOptions.SelectedIndex = 0;

            /*
            this.AddContentAmbianceOptions.ItemsSource = AudioManager.GetSoundEffects();
            this.AddContentAmbianceOptions.DisplayMemberPath = "Name";
            this.AddContentAmbianceOptions.SelectedValuePath = "Id";
            this.AddContentAmbianceOptions.SelectedIndex = 0;

            this.AddContentAmbianceOptions1.ItemsSource = AudioManager.GetSoundEffects();
            this.AddContentAmbianceOptions1.DisplayMemberPath = "Name";
            this.AddContentAmbianceOptions1.SelectedValuePath = "Id";
            this.AddContentAmbianceOptions1.SelectedIndex = 0;

            this.AddContentAmbianceOptions2.ItemsSource = AudioManager.GetSoundEffects();
            this.AddContentAmbianceOptions2.DisplayMemberPath = "Name";
            this.AddContentAmbianceOptions2.SelectedValuePath = "Id";
            this.AddContentAmbianceOptions2.SelectedIndex = 0;

            this.AddContentAmbianceOptions3.ItemsSource = AudioManager.GetSoundEffects();
            this.AddContentAmbianceOptions3.DisplayMemberPath = "Name";
            this.AddContentAmbianceOptions3.SelectedValuePath = "Id";
            this.AddContentAmbianceOptions3.SelectedIndex = 0;

            this.AddContentAmbianceOptions4.ItemsSource = AudioManager.GetSoundEffects();
            this.AddContentAmbianceOptions4.DisplayMemberPath = "Name";
            this.AddContentAmbianceOptions4.SelectedValuePath = "Id";
            this.AddContentAmbianceOptions4.SelectedIndex = 0;
            */

            this.editContentSelectionDropdown.DisplayMemberPath = "Name";
            this.editContentSelectionDropdown.SelectedValuePath = "Id";
            this.editContentSelectionDropdown.SelectedIndex = 0;

            this.removeContentSelectionDropdown.DisplayMemberPath = "Name";
            this.removeContentSelectionDropdown.SelectedValuePath = "Id";
            this.removeContentSelectionDropdown.SelectedIndex = 0;

            this.activeGrid = this.mainGrid;

            this.parentRef = appRef;

            App.BattleTab.ElementSelected += this.SelectedCreatureChanged;
        }

        /// <summary>
        /// Event handler for when the main tab of UI is changed.
        /// </summary>
        /// <exception cref="Exception">Exception thrown when event is flagged by a non-button.</exception>
        private void ScreenChangeClick(object sender, RoutedEventArgs e)
        {
            // Ensure that screen is properly reset.
            this.ContentClearAll();
            this.ListReread();

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
            else if (selected == "System.Windows.Controls.ComboBoxItem: ambiance")
            {
                this.addContentAmbianceGrid.IsEnabled = true;
                this.addContentAmbianceGrid.Visibility = Visibility.Visible;

                this.activeSubSubGrid = this.addContentAmbianceGrid;
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
            this.addContentIconAttacksBox.Text = string.Empty;
            this.addContentIconStatsBox.Text = string.Empty;
            this.AddContentIconHealthBox.Text = string.Empty;

            this.addContentOverlayOpenFileFeedback.Text = string.Empty;
            this.addContentOverlayNameBox.Text = string.Empty;
            this.addContentOverlayConfirmFeedback.Text = string.Empty;

            this.addContentMapOpenFileFeedback.Text = string.Empty;
            this.addContentMapNameBox.Text = string.Empty;
            this.AddContentMapWidthBox.Text = string.Empty;
            this.AddContentMapHeightBox.Text = string.Empty;
            this.addContentMapConfirmFeedback.Text = string.Empty;

            // edit section
            if(this.editContentMusicIntroOpenFileFeedback == null)
            {
                return;
            }

            this.editContentMusicIntroOpenFileFeedback.Text = string.Empty;
            this.editContentMusicOpenFileFeedback.Text = string.Empty;
            this.editContentMusicNameBox.Text = string.Empty;
            this.editContentMusicConfirmFeedback.Text = string.Empty;

            this.editContentSoundOpenFileFeedback.Text = string.Empty;
            this.editContentSoundNameBox.Text = string.Empty;
            this.editContentSoundConfirmFeedback.Text = string.Empty;

            this.editContentIconOpenFileFeedback.Text = string.Empty;
            this.editContentIconNameBox.Text = string.Empty;
            this.editContentIconWidthBox.Text = string.Empty;
            this.editContentIconHeightBox.Text = string.Empty;
            this.editContentIconConfirmFeedback.Text = string.Empty;
            this.editContentIconAttacksBox.Text = string.Empty;
            this.editContentIconStatsBox.Text = string.Empty;
            this.editContentIconHealthBox.Text = string.Empty;

            this.editContentOverlayOpenFileFeedback.Text = string.Empty;
            this.editContentOverlayNameBox.Text = string.Empty;
            this.editContentOverlayConfirmFeedback.Text = string.Empty;

            this.editContentMapOpenFileFeedback.Text = string.Empty;
            this.editContentMapNameBox.Text = string.Empty;
            this.editContentMapWidthBox.Text = string.Empty;
            this.editContentMapHeightBox.Text = string.Empty;
            this.editContentMapConfirmFeedback.Text = string.Empty;
        }

        private void ListReread()
        {
            OverlayManager.InitAssets();
            MapManager.InitMaps();
            AudioManager.InitTracks();

            this.musicOptions.ItemsSource = AudioManager.GetTracks();
            this.soundOptions.ItemsSource = AudioManager.GetSoundEffects();
            this.overlayOptions.ItemsSource = OverlayManager.GetOverlays();
            this.iconOptions.ItemsSource = MapManager.GetIcons();
            this.mapOptions.ItemsSource = MapManager.GetMaps();

            string type = this.removeContentTypeDropdown.SelectedValue.ToString();

            switch (type)
            {
                case "System.Windows.Controls.ComboBoxItem: music":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = AudioManager.GetTracks();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: sound":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = AudioManager.GetSoundEffects();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: overlay":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = OverlayManager.GetOverlays();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: icon":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = MapManager.GetIcons();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: map":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = MapManager.GetMaps();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: ambiance":
                    {
                        break;
                    }
            }

            type = this.editContentTypeDropdown.SelectedValue.ToString();

            switch (type)
            {
                case "System.Windows.Controls.ComboBoxItem: music":
                    {
                        this.editContentSelectionDropdown.ItemsSource = AudioManager.GetTracks();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: sound":
                    {
                        this.editContentSelectionDropdown.ItemsSource = AudioManager.GetSoundEffects();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: overlay":
                    {
                        this.editContentSelectionDropdown.ItemsSource = OverlayManager.GetOverlays();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: icon":
                    {
                        this.editContentSelectionDropdown.ItemsSource = MapManager.GetIcons();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: map":
                    {
                        this.editContentSelectionDropdown.ItemsSource = MapManager.GetMaps();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: ambiance":
                    {
                        break;
                    }
            }
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

            switch (sourceButton.Name)
            {
                case "addContentMusicOpenFileButton":
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

                        break;
                    }

                case "editContentMusicOpenFileButton":
                    {
                        result = FileManager.OpenFile("music");
                        if (result == true)
                        {
                            this.editContentMusicOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                        }
                        else
                        {
                            this.editContentMusicOpenFileFeedback.Text = "Error: could not open file";
                        }

                        break;
                    }

                case "addContentMusicIntroOpenFileButton":
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

                        break;
                    }

                case "editContentMusicIntroOpenFileButton":
                    {
                        result = FileManager.OpenIntroFile();
                        if (result == true)
                        {
                            this.editContentMusicIntroOpenFileFeedback.Text = "Current file: " + FileManager.CurrentIntroPath;
                        }
                        else
                        {
                            this.editContentMusicIntroOpenFileFeedback.Text = "Error: could not open file";
                        }

                        break;
                    }

                case "addContentSoundOpenFileButton":
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

                        break;
                    }

                case "editContentSoundOpenFileButton":
                    {
                        result = FileManager.OpenFile("sound");
                        if (result == true)
                        {
                            this.editContentSoundOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                        }
                        else
                        {
                            this.editContentSoundOpenFileFeedback.Text = "Error: could not open file";
                        }

                        break;
                    }

                case "addContentOverlayOpenFileButton":
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

                        break;
                    }

                case "editContentOverlayOpenFileButton":
                    {
                        result = FileManager.OpenFile("overlay");
                        if (result == true)
                        {
                            this.editContentOverlayOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                        }
                        else
                        {
                            this.editContentOverlayOpenFileFeedback.Text = "Error: could not open file";
                        }

                        break;
                    }

                case "addContentMapOpenFileButton":
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

                        break;
                    }

                case "editContentMapOpenFileButton":
                    {
                        result = FileManager.OpenFile("map");
                        if (result == true)
                        {
                            this.editContentMapOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                        }
                        else
                        {
                            this.editContentMapOpenFileFeedback.Text = "Error: could not open file";
                        }

                        break;
                    }

                case "addContentIconOpenFileButton":
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

                        break;
                    }

                case "editContentIconOpenFileButton":
                    {
                        result = FileManager.OpenFile("icon");
                        if (result == true)
                        {
                            this.editContentIconOpenFileFeedback.Text = "Current file: " + FileManager.CurrentFilePath;
                        }
                        else
                        {
                            this.editContentIconOpenFileFeedback.Text = "Error: could not open file";
                        }

                        break;
                    }

                default:
                    {
                        throw new Exception("unimplemented resource type");
                    }
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
            string[] data = new string[8];

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

            data[5] = this.addContentIconStatsBox.Text;
            data[6] = this.addContentIconAttacksBox.Text;

            int health = 1;
            if (this.AddContentIconHealthBox.Text.Length > 0)
            {
                if(int.TryParse(this.AddContentIconHealthBox.Text, out health))
                {
                    data[7] = System.Text.RegularExpressions.Regex.Replace(this.AddContentIconHealthBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.addContentIconConfirmFeedback.Text = "Error: please use only numbers in your health";
                    return;
                }
            }

            FileManager.CopyFile("icon");
            FileManager.AddData("icon", data);
            this.ContentClearAll();
            this.addContentIconConfirmFeedback.Text = "Successfully added!";
            MapManager.InitMaps();
            this.iconOptions.ItemsSource = MapManager.GetIcons();
            this.iconOptions.DisplayMemberPath = "Name";
            this.iconOptions.SelectedValuePath = "Id";
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
            this.overlayOptions.ItemsSource = OverlayManager.GetOverlays();
            this.overlayOptions.DisplayMemberPath = "Name";
            this.overlayOptions.SelectedValuePath = "Id";
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

        private void AddContentAmbianceAddElementClick(object sender, RoutedEventArgs e)
        {
            var newAmb = new AmbianceSettings();
            newAmb.Height = 150;
            this.AddContentAmbianceStackPanel.Children.Insert(this.AddContentAmbianceStackPanel.Children.Count - 1, newAmb);
        }

        private void AddContentAmbianceTestClick(object sender, RoutedEventArgs e)
        {
            List<AmbianceData> data = new List<AmbianceData>();
            for(int i = 0; i < this.AddContentAmbianceStackPanel.Children.Count - 1; i++)
            {
                data.Add(((AmbianceSettings)this.AddContentAmbianceStackPanel.Children[i]).ReadData());
            }

            AudioPlayer.PrepAmbianceWorkers(data);
        }

        private void AddContentAmbianceConfirmClick(object sender, RoutedEventArgs e)
        {
            //no name entered case
            if(this.addContentAmbianceName.Text == string.Empty)
            {
                return;
            }
            
            //TODO
        }

        private void EditContentTypeDropdownChanged(object sender, RoutedEventArgs e)
        {
            string selected = this.editContentTypeDropdown.SelectedValue.ToString();

            this.activeSubSubGrid.IsEnabled = false;
            this.activeSubSubGrid.Visibility = Visibility.Hidden;

            this.ContentClearAll();

            switch (selected)
            {
                case "System.Windows.Controls.ComboBoxItem: music":
                    {
                        this.editContentMusicGrid.IsEnabled = true;
                        this.editContentMusicGrid.Visibility = Visibility.Visible;

                        this.activeSubSubGrid = this.editContentMusicGrid;

                        this.EditContentUpdateDropdown("music");
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: sound":
                    {
                        this.editContentSoundGrid.IsEnabled = true;
                        this.editContentSoundGrid.Visibility = Visibility.Visible;

                        this.activeSubSubGrid = this.editContentSoundGrid;

                        this.EditContentUpdateDropdown("sound");
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: overlay":
                    {
                        this.editContentOverlayGrid.IsEnabled = true;
                        this.editContentOverlayGrid.Visibility = Visibility.Visible;

                        this.activeSubSubGrid = this.editContentOverlayGrid;

                        this.EditContentUpdateDropdown("overlay");
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: icon":
                    {
                        this.editContentIconGrid.IsEnabled = true;
                        this.editContentIconGrid.Visibility = Visibility.Visible;

                        this.activeSubSubGrid = this.editContentIconGrid;

                        this.EditContentUpdateDropdown("icon");
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: map":
                    {
                        this.editContentMapGrid.IsEnabled = true;
                        this.editContentMapGrid.Visibility = Visibility.Visible;

                        this.activeSubSubGrid = this.editContentMapGrid;

                        this.EditContentUpdateDropdown("map");
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: ambiance":
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
                
        }

        private void EditContentUpdateDropdown(string type)
        {
            this.editContentSelectionDropdown.DisplayMemberPath = "Name";
            this.editContentSelectionDropdown.SelectedValuePath = "Id";
            switch (type)
            {
                case "music":
                    {
                        this.editContentSelectionDropdown.ItemsSource = AudioManager.GetTracks();
                        break;
                    }

                case "sound":
                    {
                        this.editContentSelectionDropdown.ItemsSource = AudioManager.GetSoundEffects();
                        break;
                    }

                case "overlay":
                    {
                        this.editContentSelectionDropdown.ItemsSource = OverlayManager.GetOverlays();
                        break;
                    }

                case "icon":
                    {
                        this.editContentSelectionDropdown.ItemsSource = MapManager.GetIcons();
                        break;
                    }

                case "map":
                    {
                        this.editContentSelectionDropdown.ItemsSource = MapManager.GetMaps();
                        break;
                    }

                case "ambiance":
                    {
                        break;
                    }
            }

            this.editContentSelectionDropdown.SelectedIndex = 0;
        }

        private void EditContentSelectionDropdownChanged(object sender, RoutedEventArgs e)
        {
            if(this.editContentSelectionDropdown.SelectedValue == null)
            {
                return;
            }

            switch (this.editContentTypeDropdown.SelectedValue.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: music":
                    {
                        string[] songPaths = AudioManager.GetTrackPath(this.editContentSelectionDropdown.SelectedValue.ToString());
                        FileManager.ClearBuffers();
                        FileManager.CurrentFilePath = songPaths[0];
                        FileManager.CurrentIntroPath = songPaths[1];

                        this.editContentMusicOpenFileFeedback.Text = "Current file: " + songPaths[0];
                        this.editContentMusicIntroOpenFileFeedback.Text = "Current file: " + songPaths[1];

                        this.editContentMusicNameBox.Text = AudioManager.GetTrackTitle(this.editContentSelectionDropdown.SelectedValue.ToString());

                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: sound":
                    {
                        string path = AudioManager.GetSoundEffectPath(this.editContentSelectionDropdown.SelectedValue.ToString());
                        FileManager.ClearBuffers();
                        FileManager.CurrentFilePath = path;

                        this.editContentSoundOpenFileFeedback.Text = "Current file: " + path;

                        this.editContentSoundNameBox.Text = AudioManager.GetSoundTitle(this.editContentSelectionDropdown.SelectedValue.ToString());

                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: overlay":
                    {
                        string path = OverlayManager.GetOverlayPath(this.editContentSelectionDropdown.SelectedValue.ToString());
                        FileManager.ClearBuffers();
                        FileManager.CurrentFilePath = path;

                        this.editContentOverlayOpenFileFeedback.Text = "Current file: " + path;

                        this.editContentOverlayNameBox.Text = OverlayManager.GetOverlayName(this.editContentSelectionDropdown.SelectedValue.ToString());

                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: icon":
                    {
                        Icon icon = MapManager.GetLoadedIcon(this.editContentSelectionDropdown.SelectedValue.ToString());
                        FileManager.ClearBuffers();
                        FileManager.CurrentFilePath = icon.FilePath;

                        this.editContentIconOpenFileFeedback.Text = "Current file: " + icon.FilePath;

                        this.editContentIconAttacksBox.Text = icon.Attacks;
                        this.editContentIconStatsBox.Text = icon.Stats;
                        this.editContentIconHealthBox.Text = icon.MaxHealth.ToString();
                        this.editContentIconHeightBox.Text = icon.Height.ToString();
                        this.editContentIconWidthBox.Text = icon.Width.ToString();
                        this.editContentIconNameBox.Text = icon.Name;

                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: map":
                    {
                        Map map = MapManager.GetLoadedMap(this.editContentSelectionDropdown.SelectedValue.ToString());
                        FileManager.ClearBuffers();
                        FileManager.CurrentFilePath = map.FilePath;

                        this.editContentMapOpenFileFeedback.Text = map.FilePath;

                        this.editContentMapHeightBox.Text = map.Height.ToString();
                        this.editContentMapWidthBox.Text = map.Width.ToString();
                        this.editContentMapNameBox.Text = map.Name;

                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: ambiance":
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

        }

        private void EditContentMusicConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[3];

            if (this.editContentMusicNameBox.Text.Length > 0)
            {
                data[0] = this.editContentMusicNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.addContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.editContentMusicConfirmFeedback.Text = "Error: no track name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.editContentMusicConfirmFeedback.Text = "Error: no loop file selected";
                return;
            }

            if (FileManager.CurrentIntroPath != string.Empty)
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

            FileManager.AddData("music", data, this.editContentSelectionDropdown.SelectedValue.ToString());
            this.ContentClearAll();
            this.editContentMusicConfirmFeedback.Text = "Successfully updated!";
            this.ListReread();
        }

        private void EditContentSoundConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[2];

            if (this.editContentSoundNameBox.Text.Length > 0)
            {
                data[0] = this.editContentSoundNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.editContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.editContentSoundConfirmFeedback.Text = "Error: no sound name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.editContentSoundConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            FileManager.CopyFile("sound");

            FileManager.AddData("sound", data, this.editContentSelectionDropdown.SelectedValue.ToString());
            this.ContentClearAll();
            this.editContentSoundConfirmFeedback.Text = "Successfully updated!";
            this.ListReread();
        }

        private void EditContentIconConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[8];

            if (this.editContentIconNameBox.Text.Length > 0)
            {
                data[0] = this.editContentIconNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.editContentIconConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.editContentIconConfirmFeedback.Text = "Error: no name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.editContentIconConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            if (Path.GetExtension(FileManager.CurrentFilePath) == ".gif")
            {
                data[2] = "ANIMATED";
            }
            else
            {
                data[2] = "STATIC";
            }

            int width = 1;
            if (this.editContentIconWidthBox.Text.Length > 0)
            {
                if (int.TryParse(this.editContentIconWidthBox.Text, out width))
                {
                    data[3] = System.Text.RegularExpressions.Regex.Replace(this.editContentIconWidthBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.editContentIconConfirmFeedback.Text = "Error: please use only numbers in your width, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.editContentIconConfirmFeedback.Text = "Error: no width provided";
                return;
            }

            int height = 1;
            if (this.editContentIconHeightBox.Text.Length > 0)
            {
                if (int.TryParse(this.editContentIconHeightBox.Text, out height))
                {
                    data[4] = System.Text.RegularExpressions.Regex.Replace(this.editContentIconHeightBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.editContentIconConfirmFeedback.Text = "Error: please use only numbers in your height, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.editContentIconConfirmFeedback.Text = "Error: no height provided";
                return;
            }

            data[5] = this.editContentIconStatsBox.Text;
            data[6] = this.editContentIconAttacksBox.Text;

            int health = 1;
            if (this.editContentIconHealthBox.Text.Length > 0)
            {
                if (int.TryParse(this.editContentIconHealthBox.Text, out health))
                {
                    data[7] = System.Text.RegularExpressions.Regex.Replace(this.editContentIconHealthBox.Text, @"\s", string.Empty);
                }
                else
                {
                    this.editContentIconConfirmFeedback.Text = "Error: please use only numbers in your health";
                    return;
                }
            }

            FileManager.CopyFile("icon");
            FileManager.AddData("icon", data, this.editContentSelectionDropdown.SelectedValue.ToString());
            this.ContentClearAll();
            this.editContentIconConfirmFeedback.Text = "Successfully added!";
            this.ListReread();
        }

        private void EditContentOverlayConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[5];
            if (this.editContentOverlayNameBox.Text.Length > 0)
            {
                data[0] = this.editContentOverlayNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.editContentOverlayConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.editContentOverlayConfirmFeedback.Text = "Error: no name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.editContentOverlayConfirmFeedback.Text = "Error: no file provided";
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
            FileManager.AddData("overlay", data, this.editContentSelectionDropdown.SelectedValue.ToString());
            this.ContentClearAll();
            this.editContentOverlayConfirmFeedback.Text = "Successfully added!";
            this.ListReread();
        }

        private void EditContentMapConfirmClick(object sender, RoutedEventArgs e)
        {
            string[] data = new string[4];

            if (this.editContentMapNameBox.Text.Length > 0)
            {
                data[0] = this.editContentMapNameBox.Text;
                if (data[0].Contains(','))
                {
                    this.editContentMapConfirmFeedback.Text = "Error: names may not contain the character ','";
                    return;
                }
            }
            else
            {
                this.editContentMapConfirmFeedback.Text = "Error: no name provided";
                return;
            }

            if (FileManager.CurrentFilePath != string.Empty)
            {
                data[1] = Path.GetFileName(FileManager.CurrentFilePath);
            }
            else
            {
                this.editContentMapConfirmFeedback.Text = "Error: no file provided";
                return;
            }

            int width = 1;
            if (this.editContentMapWidthBox.Text.Length > 0)
            {
                if (int.TryParse(this.editContentMapWidthBox.Text, out width))
                {
                    if (width > 400)
                    {
                        this.editContentMapConfirmFeedback.Text = "Error: please use a width of 400 tiles or lower";
                        return;
                    }
                    else
                    {
                        data[2] = System.Text.RegularExpressions.Regex.Replace(this.editContentMapWidthBox.Text, @"\s", string.Empty);
                    }
                }
                else
                {
                    this.editContentMapConfirmFeedback.Text = "Error: please use only numbers in your width, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.editContentMapConfirmFeedback.Text = "Error: no width provided";
                return;
            }

            int height = 1;
            if (this.editContentMapHeightBox.Text.Length > 0)
            {
                if (int.TryParse(this.editContentMapHeightBox.Text, out height))
                {
                    if (height > 400)
                    {
                        this.editContentMapConfirmFeedback.Text = "Error: please use a height of 400 tiles or lower";
                        return;
                    }
                    else
                    {
                        data[3] = System.Text.RegularExpressions.Regex.Replace(this.editContentMapHeightBox.Text, @"\s", string.Empty);
                    }
                }
                else
                {
                    this.editContentMapConfirmFeedback.Text = "Error: please use only numbers in your height, (for now whole numbers only)";
                    return;
                }
            }
            else
            {
                this.editContentMapConfirmFeedback.Text = "Error: no height provided";
                return;
            }

            FileManager.CopyFile("map");
            FileManager.AddData("map", data, this.editContentSelectionDropdown.SelectedValue.ToString());
            this.ContentClearAll();
            this.editContentMapConfirmFeedback.Text = "Successfully added!";
            this.ListReread();
        }

        private void RemoveContentTypeDropdownChanged(object sender, RoutedEventArgs e)
        {
            if (this.removeContentTypeDropdown == null || this.removeContentSelectionDropdown == null)
            {
                return;
            }

            this.removeContentSelectionDropdown.DisplayMemberPath = "Name";
            this.removeContentSelectionDropdown.SelectedValuePath = "Id";

            string type = this.removeContentTypeDropdown.SelectedValue.ToString();

            switch (type)
            {
                case "System.Windows.Controls.ComboBoxItem: music":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = AudioManager.GetTracks();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: sound":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = AudioManager.GetSoundEffects();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: overlay":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = OverlayManager.GetOverlays();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: icon":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = MapManager.GetIcons();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: map":
                    {
                        this.removeContentSelectionDropdown.ItemsSource = MapManager.GetMaps();
                        break;
                    }

                case "System.Windows.Controls.ComboBoxItem: ambiance":
                    {
                        break;
                    }
            }

            this.removeContentSelectionDropdown.SelectedIndex = 0;
        }

        private void RemoveButtonDeleteClicked(object sender, RoutedEventArgs e)
        {
            string? id = this.removeContentSelectionDropdown.SelectedValue.ToString();

            StorageManager.DeleteObject(id);

            this.ContentClearAll();
            this.ListReread();
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

        private void CurrentNameChanged(object sender, RoutedEventArgs e)
        {
            App.BattleTab.UpdateName(this.statsGridCreatureName.Text);
        }

        private void CurrentHealthChanged(object sender, RoutedEventArgs e)
        {
            App.BattleTab.UpdateHealth(this.statsGridCreatureCurrentHealth.Text);
        }

        private void SelectedCreatureChanged(object selected)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (selected.GetType() == typeof(CreatureIcon))
                {
                    CreatureIcon icon = (CreatureIcon)selected;
                    string[] stats = icon.GetStats();
                    this.statsGridCreatureName.TextChanged -= this.CurrentNameChanged;
                    this.statsGridCreatureName.Text = stats[0];
                    this.statsGridCreatureName.TextChanged += this.CurrentNameChanged;

                    this.statsGridCreatureStats.Text = stats[1];
                    this.statsGridCreatureAbilities.Text = stats[2];
                    this.statsGridCreatureMaxHealth.Text = stats[3];

                    this.statsGridCreatureCurrentHealth.TextChanged -= this.CurrentHealthChanged;
                    this.statsGridCreatureCurrentHealth.Text = stats[4];
                    this.statsGridCreatureCurrentHealth.TextChanged += this.CurrentHealthChanged;
                }
                else
                {
                    this.statsGridCreatureName.TextChanged -= this.CurrentNameChanged;
                    this.statsGridCreatureName.Text = string.Empty;
                    this.statsGridCreatureName.TextChanged += this.CurrentNameChanged;

                    this.statsGridCreatureStats.Text = string.Empty;
                    this.statsGridCreatureAbilities.Text = string.Empty;
                    this.statsGridCreatureMaxHealth.Text = string.Empty;

                    this.statsGridCreatureCurrentHealth.TextChanged -= this.CurrentHealthChanged;
                    this.statsGridCreatureCurrentHealth.Text = string.Empty;
                    this.statsGridCreatureCurrentHealth.TextChanged += this.CurrentHealthChanged;
                }
            });
        }
    }
}
