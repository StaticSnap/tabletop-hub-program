// <copyright file="OverlayScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
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

    /// <summary>
    /// The overlay screen for the hub. allows for image and video functionality. TODO.
    /// </summary>
    public partial class OverlayScreen : Window
    {
        private static bool editing = true;

        private static UIElement? screenElement;

        private static MediaElement overlayVideo = new MediaElement();

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayScreen"/> class.
        /// </summary>
        public OverlayScreen()
        {
            this.InitializeComponent();
            this.Topmost = true;
            this.KeyDown += this.OnKeyDown;

            this.ChangeWindowState();
        }

        /// <summary>
        /// Closes this window.
        /// </summary>
        public void CloseWindow()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Close();
            });
        }

        /// <summary>
        /// Toggle class to switch between invisible and movable.
        /// </summary>
        public void ChangeWindowState()
        {
            this.Dispatcher.Invoke(() =>
            {
                // Lock in overlay window and hide it.
                if (editing == true)
                {
                    SolidColorBrush brush = new SolidColorBrush { Opacity = 0, Color = Colors.White };

                    this.grid.Children.Clear();

                    this.Background = brush;

                    this.WindowState = WindowState.Maximized;

                    if(screenElement != null)
                    {
                        this.grid.Children.Add(screenElement);
                    }

                    editing = false;
                }

                // Unlock overlay and allow for movement.
                else
                {
                    SolidColorBrush brush = new SolidColorBrush { Opacity = 1, Color = Colors.Blue };

                    this.Background = brush;

                    this.WindowState = WindowState.Normal;

                    this.grid.Children.Clear();

                    TextBlock text = new TextBlock { Text = "Move Me!", FontSize = 60, TextAlignment = TextAlignment.Center };

                    this.grid.Children.Add(text);

                    editing = true;
                }
            });
        }

        /// <summary>
        /// Begins showing the given overlay element.
        /// </summary>
        /// <param name="elementName">Name of the element to display.</param>
        public void EnableOverlayElement(string elementName)
        {
            if (!editing)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Dictionary<string, string[]> overlayDat = OverlayManager.GetOverlayObjects();

                    if (overlayDat.ContainsKey(elementName))
                    {
                        this.DisableOverlayElement();
                        switch (overlayDat[elementName][2])
                        {
                            // Handle each of the file types seperatly
                            case "IMAGE":
                                this.EnableImage(overlayDat[elementName]);
                                break;
                            case "VIDEO":
                                this.EnableVideo(overlayDat[elementName]);
                                break;
                            case "GIF":
                                this.EnableGif(overlayDat[elementName]);
                                break;
                            default:
                                throw new Exception("unrecognized file format");
                        }
                    }
                    else
                    {
                        throw new Exception("unrecognized overlay name");
                    }
                });
            }
        }

        /// <summary>
        /// Cuts off the currently playing element.
        /// </summary>
        public void DisableOverlayElement()
        {
            if (!editing)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.grid.Children.Clear();
                    screenElement = null;
                });
            }
        }

        /// <summary>
        /// Tells App that the window has been closed so no functions are called on it.
        /// </summary>
        private void OverlayScreenClosing(object sender, CancelEventArgs e)
        {
            App.OverlayRunning = false;
        }

        /// <summary>
        /// Reads the image data and puts the image on the overlay.
        /// </summary>
        /// <param name="imageDat">String of data required to open image.</param>
        /// <exception cref="Exception">I haven't written the greenscreen.</exception>
        private void EnableImage(string[] imageDat)
        {
            Image overlay = new Image();
            if (imageDat[4] == "NULL")
            {
                BitmapImage imageFile = new BitmapImage();

                imageFile.BeginInit();

                imageFile.UriSource = new Uri(OverlayManager.GetOverlayPath(imageDat[0]));

                imageFile.EndInit();

                overlay.Source = imageFile;

                screenElement = overlay;

                this.grid.Children.Add(screenElement);
            }
            else
            {
                //case for green screen images. implement later
                throw new Exception("green screen images not implemented");
            }
        }

        /// <summary>
        /// Reads the video data and begins playback.
        /// </summary>
        /// <param name="videoDat">Data to open videos.</param>
        private void EnableVideo(string[] videoDat)
        {
            //makes sure that spamming start video doesn't stack events.
            overlayVideo.MediaEnded -= this.DisposeVideo;

            // looping video.
            if (videoDat[3] == "TRUE")
            {
                // video has no green screen.
                if(videoDat[4] == "NULL")
                {
                }

                // video uses a green screen.
                else
                {
                }
            }

            // video plays once.
            else
            {
                // video has no green screen.
                if(videoDat[4] == "NULL")
                {
                    overlayVideo.LoadedBehavior = MediaState.Manual;
                    overlayVideo.Source = new Uri(OverlayManager.GetOverlayPath(videoDat[0]), UriKind.Absolute);

                    screenElement = overlayVideo;

                    this.grid.Children.Add(screenElement);

                    overlayVideo.Play();

                    overlayVideo.MediaEnded += this.DisposeVideo;
                }

                //video uses a green screen.
                else
                {
                }
            }
        }

        private void EnableGif(string[] gifDat)
        {
            // TODO
        }

        private void OverlayMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && editing == true)
            {
                this.DragMove();
            }
        }

        private void DisposeVideo(object sender, EventArgs e)
        {
            this.grid.Children.Clear();

            overlayVideo.MediaEnded -= this.DisposeVideo;

            overlayVideo.Close();

            overlayVideo = new MediaElement();
        }

        // KeyDown event to handle the Delete key
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Prevents user from softlocking themselves incase the cover entire screen in overlay.
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                this.DisableOverlayElement();
            }
        }
    }
}
