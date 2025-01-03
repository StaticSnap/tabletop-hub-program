﻿// <copyright file="BattleMapScreen.xaml.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for BattleMapScreen.xaml.
    /// </summary>
    public partial class BattleMapScreen : Window
    {
        private Image mapImage = new Image();
        private Grid mapGrid = new Grid();

        private bool isDragging = false;
        private Point clickPosition;
        private UIElement? draggedElement = null;  // This will hold the dragged control

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleMapScreen"/> class.
        /// </summary>
        public BattleMapScreen()
        {
            this.InitializeComponent();

            MapManager.InitMaps();

            this.mapGrid.ShowGridLines = false;

            this.OpenMap("abandoned city");

            this.AddCreature("galio");
            this.AddCreature("rob");
            this.AddCreature("daos");
            this.AddCreature("farren");
            this.AddCreature("ellie");
            this.AddCreature("coco");

            this.mapGrid.MouseDown += this.MapGridMouseDown;
            this.mapGrid.MouseMove += this.MapGridMouseMove;
            this.mapGrid.MouseUp += this.MapGridMouseUp;

            this.mapGrid.AllowDrop = true;
            this.mapGrid.Background = Brushes.Transparent;
        }

        /// <summary>
        /// Helper function to take a creature name and add it to the map.
        /// </summary>
        /// <param name="creatureName">name of the creature.</param>
        public void AddCreature(string creatureName)
        {
            CreatureIcon creature = new CreatureIcon();
            creature.ChangeIcon(creatureName);
            Ellipse creatureIcon = creature.GetIcon();
            Grid.SetColumn(creatureIcon, 0);
            Grid.SetRow(creatureIcon, 0);
            this.mapGrid.Children.Add(creatureIcon);
        }

        /// <summary>
        /// take a map name and open it for use.
        /// </summary>
        /// <param name="mapName">name of the map.</param>
        public void OpenMap(string mapName)
        {
            this.canvas.Children.Clear();

            string[] mapData = MapManager.GetMaps()[mapName];

            int width = -1;
            int height = -1;

            int.TryParse(mapData[2], out width);
            int.TryParse(mapData[3], out height);

            BitmapImage bitMap = new BitmapImage();

            bitMap.BeginInit();
            bitMap.UriSource = new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "resources\\textures\\maps\\", mapData[1]));

            bitMap.DecodePixelWidth = 70 * width;
            bitMap.EndInit();
            
            this.mapImage.Source = bitMap;
            this.mapImage.Height = 100 * height;
            this.mapImage.Width = 100 * width;

            this.mapGrid.Height = 100 * height;
            this.mapGrid.Width = 100 * width;

            for(int i = 0; i < width; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                this.mapGrid.ColumnDefinitions.Add(col);
            }

            for (int i = 0; i < height; i++)
            {
                RowDefinition row = new RowDefinition();
                this.mapGrid.RowDefinitions.Add(row);
            }

            this.canvas.Children.Add(this.mapImage);
            this.canvas.Children.Add(this.mapGrid);
        }

        // MouseDown event to start the drag
        private void MapGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // Check if the mouse is over a control
                var element = this.GetControlAtMousePosition(e.GetPosition((UIElement)sender));

                if (element != null)
                {
                    // Start dragging the element
                    this.isDragging = true;
                    this.draggedElement = element;
                    this.clickPosition = e.GetPosition(this.draggedElement);  // Get the click position on the control
                    this.draggedElement.CaptureMouse();  // Capture mouse to the control
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
                //draggedElement.Margin = new Thickness(newLeft, newTop, 0, 0);
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
                int row = (int)(mousePos.Y / this.draggedElement.RenderSize.Height);  // Simple row calculation based on height
                int column = (int)(mousePos.X / this.draggedElement.RenderSize.Width);  // Simple column calculation based on width

                // Ensure the row and column are within bounds
                row = Math.Min(row, this.mapGrid.RowDefinitions.Count - 1);
                column = Math.Min(column, this.mapGrid.ColumnDefinitions.Count - 1);

                // Set the control's position in the grid
                Grid.SetRow(this.draggedElement, row);
                Grid.SetColumn(this.draggedElement, column);

                this.draggedElement = null;  // Clear the dragged element
            }
        }

        // Helper function to get the control at the mouse position
        private UIElement? GetControlAtMousePosition(Point mousePosition)
        {
            // Perform the hit test in the grid's coordinate system
            var hitTestResult = VisualTreeHelper.HitTest(this.mapGrid, mousePosition);

            return hitTestResult?.VisualHit as UIElement;
        }
    }
}
