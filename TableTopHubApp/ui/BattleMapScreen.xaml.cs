// <copyright file="BattleMapScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Media.Media3D;
    using System.Windows.Shapes;
    using WpfAnimatedGif;

    /// <summary>
    /// Interaction logic for BattleMapScreen.xaml.
    /// </summary>
    public partial class BattleMapScreen : Window
    {
        private Grid mapGrid = new Grid();
        private (string, string) sizeDat = ("0","0");

        private bool isDragging = false;
        private Point clickPosition;
        private UIElement? draggedElement = null;  // This will hold the dragged control
        private UIElement? selectedElement = null; // This stores the currently selected control

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleMapScreen"/> class.
        /// </summary>
        public BattleMapScreen()
        {
            this.InitializeComponent();

            this.mapGrid.ShowGridLines = true;

            this.mapGrid.MouseDown += this.MapGridMouseDown;
            this.mapGrid.MouseMove += this.MapGridMouseMove;
            this.mapGrid.MouseUp += this.MapGridMouseUp;
            this.KeyDown += this.OnKeyDown;

            this.mapGrid.AllowDrop = true;
            this.mapGrid.Background = Brushes.Transparent;
        }

        /// <summary>
        /// Helper function to take a creature name and add it to the map.
        /// </summary>
        /// <param name="creatureName">name of the creature.</param>
        public void AddCreature(string creatureName)
        {
            this.Dispatcher.Invoke(() =>
            {
                CreatureIcon creature = new CreatureIcon();
                creature.ChangeIcon(creatureName);
                Ellipse creatureIcon = creature.GetIcon();
                Grid.SetColumn(creatureIcon, creature.GetIconWidth());
                Grid.SetRow(creatureIcon, creature.GetIconHeight());
                Grid.SetColumnSpan(creatureIcon, creature.GetIconWidth());
                Grid.SetRowSpan(creatureIcon, creature.GetIconHeight());
                this.mapGrid.Children.Add(creatureIcon);
            });
        }

        /// <summary>
        /// take a map name and open it for use.
        /// </summary>
        /// <param name="mapId">name of the map.</param>
        public void OpenMap(string mapId)
        {
            this.Dispatcher.Invoke(() => 
            { 
                Map map = MapManager.GetMap(mapId);

                if (map.Width == this.sizeDat.Item1 && map.Height == this.sizeDat.Item2)
                {
                    int width = -1;
                    int height = -1;

                    int.TryParse(map.Width, out width);
                    int.TryParse(map.Height, out height);

                    if (map.Type == "ANIMATED")
                    {
                        VisualBrush brush = new VisualBrush();

                        BitmapImage uri = new BitmapImage();
                        uri.BeginInit();

                        uri.UriSource = new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps\\", map.FilePath));

                        uri.EndInit();

                        Image temp = new Image();

                        ImageBehavior.SetAnimatedSource(temp, uri);

                        brush.Visual = temp;

                        this.mapGrid.Background = brush;
                    }
                    else
                    {
                        BitmapImage bitMap = new BitmapImage();

                        bitMap.BeginInit();
                        bitMap.UriSource = new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps\\", map.FilePath));

                        bitMap.DecodePixelHeight = 50 * height;
                        bitMap.DecodePixelWidth = 50 * width;

                        bitMap.EndInit();

                        ImageBrush brush = new ImageBrush();
                        brush.ImageSource = bitMap;

                        this.mapGrid.Background = brush;
                    }
                }
                else
                {
                    this.canvas.Children.Clear();

                    this.mapGrid.Children.Clear();

                    this.mapGrid.MouseDown -= this.MapGridMouseDown;
                    this.mapGrid.MouseMove -= this.MapGridMouseMove;
                    this.mapGrid.MouseUp -= this.MapGridMouseUp;

                    this.mapGrid = new Grid();

                    this.mapGrid.ShowGridLines = false;

                    this.mapGrid.MouseDown += this.MapGridMouseDown;
                    this.mapGrid.MouseMove += this.MapGridMouseMove;
                    this.mapGrid.MouseUp += this.MapGridMouseUp;

                    this.mapGrid.AllowDrop = true;
                    this.mapGrid.Background = Brushes.Transparent;

                    int width = -1;
                    int height = -1;

                    int.TryParse(map.Width, out width);
                    int.TryParse(map.Height, out height);

                    this.sizeDat.Item1 = map.Width;
                    this.sizeDat.Item2 = map.Height;

                    if (map.Type == "ANIMATED")
                    {
                        VisualBrush brush = new VisualBrush();

                        BitmapImage uri = new BitmapImage();
                        uri.BeginInit();

                        uri.UriSource = new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps\\", map.FilePath));

                        uri.EndInit();

                        Image temp = new Image();

                        ImageBehavior.SetAnimatedSource(temp, uri);

                        brush.Visual = temp;

                        this.mapGrid.Background = brush;
                    }
                    else
                    {
                        BitmapImage bitMap = new BitmapImage();

                        bitMap.BeginInit();
                        bitMap.UriSource = new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps\\", map.FilePath));

                        bitMap.DecodePixelHeight = 50 * height;
                        bitMap.DecodePixelWidth = 50 * width;

                        bitMap.EndInit();

                        ImageBrush brush = new ImageBrush();
                        brush.ImageSource = bitMap;

                        this.mapGrid.Background = brush;
                    }

                    this.mapGrid.Height = 50 * height;
                    this.mapGrid.Width = 50 * width;

                    this.mapGrid.ColumnDefinitions.Clear();
                    for (int i = 0; i < width; i++)
                    {
                        ColumnDefinition col = new ColumnDefinition();
                        this.mapGrid.ColumnDefinitions.Add(col);
                    }

                    this.mapGrid.RowDefinitions.Clear();
                    for (int i = 0; i < height; i++)
                    {
                        RowDefinition row = new RowDefinition();
                        this.mapGrid.RowDefinitions.Add(row);
                    }

                    this.canvas.Children.Add(this.mapGrid);
                }
            });
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

        private void BattleMapClosing(object sender, CancelEventArgs e)
        {
            App.BattleRunning = false;
        }

        // MouseDown event to start the drag
        private void MapGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.draggedElement = null;
                this.selectedElement = null;

                // Check if the mouse is over a control
                var element = this.GetControlAtMousePosition(e.GetPosition((UIElement)sender));

                if (element != null)
                {
                    // Start dragging the element
                    this.isDragging = true;
                    this.draggedElement = element;
                    this.clickPosition = e.GetPosition(this.draggedElement);  // Get the click position on the control
                    this.draggedElement.CaptureMouse();  // Capture mouse to the control

                    this.selectedElement = element;
                }
            }
        }

        // MouseMove event to move the control
        private void MapGridMouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging && this.draggedElement != null)
            {
                // Calculate the new position based on mouse movement
                var mousePos = e.GetPosition((UIElement)sender);
                var newLeft = mousePos.X - this.clickPosition.X;
                var newTop = mousePos.Y - this.clickPosition.Y;

                // Update the margin of the dragged control to move it
                // draggedElement.Margin = new Thickness(newLeft, newTop, 0, 0);
            }
        }

        // MouseUp event to stop the drag
        private void MapGridMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.isDragging && this.draggedElement != null)
            {
                // Stop dragging and release mouse capture
                this.isDragging = false;
                this.draggedElement.ReleaseMouseCapture();

                // Get the final mouse position
                var mousePos = e.GetPosition((UIElement)sender);

                // Calculate the new row and column based on the mouse position
                int row = (int)(mousePos.Y / this.draggedElement.RenderSize.Height * Grid.GetRowSpan(this.draggedElement));  // Simple row calculation based on height
                int column = (int)(mousePos.X / this.draggedElement.RenderSize.Width * Grid.GetColumnSpan(this.draggedElement));  // Simple column calculation based on width

                // Ensure the row and column are within bounds
                row = Math.Min(row, this.mapGrid.RowDefinitions.Count - 1);
                column = Math.Min(column, this.mapGrid.ColumnDefinitions.Count - 1);

                // Set the control's position in the grid
                Grid.SetRow(this.draggedElement, row);
                Grid.SetColumn(this.draggedElement, column);
            }
        }

        // Helper function to get the control at the mouse position
        private UIElement? GetControlAtMousePosition(Point mousePosition)
        {
            // Perform the hit test in the grid's coordinate system
            var hitTestResult = VisualTreeHelper.HitTest(this.mapGrid, mousePosition);

            return hitTestResult?.VisualHit as UIElement;
        }

        // KeyDown event to handle the Delete key
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (this.selectedElement != null)
                {
                    this.mapGrid.Children.Remove(this.selectedElement);
                    this.selectedElement = null;  // Clear selected element after deletion
                }
            }
        }
    }
}
