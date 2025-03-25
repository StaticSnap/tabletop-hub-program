// <copyright file="App.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.Configuration;
    using System.Data;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        private static MusicScreen musicTab;
        private static BattleMapScreen battleTab;
        private static OverlayScreen overlayTab;

        private static Thread musicThread;
        private static Thread battleThread;
        private static Thread overlayThread;

        private static bool musicRunning;
        private static bool battleRunning;
        private static bool overlayRunning;


        /// <summary>
        /// Gets or sets the music tab.
        /// </summary>
        public static MusicScreen MusicTab
        {
            get => musicTab;
            set => musicTab = value;
        }

        /// <summary>
        /// Gets or sets the battle tab.
        /// </summary>
        public static BattleMapScreen BattleTab
        {
            get => battleTab;
            set => battleTab = value;
        }

        /// <summary>
        /// Gets or sets the ovewrlay tab.
        /// </summary>
        public static OverlayScreen OverlayTab
        {
            get => overlayTab;
            set => overlayTab = value;
        }

        public static bool MusicRunning
        {
            get => musicRunning;
            set => musicRunning = value;
        }

        public static bool BattleRunning
        {
            get => battleRunning;
            set => battleRunning = value;
        }

        public static bool OverlayRunning
        {
            get => overlayRunning;
            set => overlayRunning = value;
        }

        /// <summary>
        /// Method called by other windows to ensure app actually stops running on close.
        /// </summary>
        public void EndApp()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Shutdown();
            });
        }

        public void BattleThreadKill()
        {
            if (battleThread.IsAlive)
            {
                battleTab.CloseWindow();
                battleRunning = false;
            }
        }

        public void OverlayThreadKill()
        {
            if (overlayThread.IsAlive)
            {
                overlayTab.CloseWindow();
                overlayRunning = false;
            }
        }

        /// <summary>
        /// Ran when the application is first run.
        /// </summary>
        /// <param name="e">Arguements for when program opens.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            musicThread = new Thread(this.StartMusicWindow);
            musicThread.SetApartmentState(ApartmentState.STA);
            musicThread.Start();

            battleThread = new Thread(this.StartBattleWindow);
            battleThread.SetApartmentState(ApartmentState.STA);
            battleThread.Start();

            overlayThread = new Thread(this.StartOverlayWindow);
            overlayThread.SetApartmentState(ApartmentState.STA);
            overlayThread.Start();
        }

        private void StartMusicWindow()
        {
            musicTab = new MusicScreen(this);
            musicRunning = true;
            this.Dispatcher.Invoke(() =>
            {
                Current.MainWindow = musicTab;
            });
            musicTab.ShowDialog();
        }

        private void StartBattleWindow()
        {
            battleTab = new BattleMapScreen();
            battleRunning = true;
            battleTab.ShowDialog();
        }

        private void StartOverlayWindow()
        {
            overlayTab = new OverlayScreen();
            overlayRunning = true;
            overlayTab.ShowDialog();
        }
    }
}
