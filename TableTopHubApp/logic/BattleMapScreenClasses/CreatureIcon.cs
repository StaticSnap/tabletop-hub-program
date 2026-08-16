// <copyright file="CreatureIcon.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.IO;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using WpfAnimatedGif;

    /// <summary>
    /// Creature Icon class contains the methods required to create a circular creature for the board. 
    /// </summary>
    internal class CreatureIcon : System.Windows.UIElement
    {
        private ImageBrush iconImage = new ImageBrush();
        private VisualBrush iconGif = new VisualBrush();
        private Ellipse fullIcon = new Ellipse();
        private Icon sourceIcon = new Icon();
        private Creature stats = new Creature(string.Empty, string.Empty, string.Empty, 0);
        private bool selected = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatureIcon"/> class.
        /// </summary>
        public CreatureIcon()
        {
            this.fullIcon.Stroke = Brushes.Black;
            this.fullIcon.StrokeThickness = 3;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the icon is selected.
        /// </summary>
        public bool Selected
        {
            get => this.selected;
            set
            {  
                this.selected = value;
                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// Take in the name of an icon and open the correct file and set it to be in the ellipse.
        /// </summary>
        /// <param name="id">name of the image.</param>
        public void ChangeIcon(string id)
        {
            Icon? attemptLoad = StorageManager.LoadIconObject(id);

            if(attemptLoad == null)
            {
                throw new Exception("Icon not found.");
            }

            this.sourceIcon = attemptLoad;

            BitmapImage uri = new BitmapImage();
            uri.BeginInit();

            uri.UriSource = new Uri(MapManager.GetIconPath(this.sourceIcon));

            uri.EndInit();

            if (this.sourceIcon.Type == "ANIMATED")
            {
                Image temp = new Image();

                ImageBehavior.SetAnimatedSource(temp, uri);

                this.iconGif.Visual = temp;

                this.fullIcon.Fill = this.iconGif;
            }
            else
            {
                this.iconImage.ImageSource = uri;

                this.fullIcon.Fill = this.iconImage;
            }

            this.stats = new Creature(this.sourceIcon.Name, this.sourceIcon.Stats, this.sourceIcon.Attacks, this.sourceIcon.MaxHealth);
        }

        /// <summary>
        /// Gets the ellipse to display on the map.
        /// </summary>
        /// <returns>an Ellipse object.</returns>
        public Ellipse GetIcon()
        {
            return this.fullIcon;
        }

        /// <summary>
        /// Gets the width in cells that the icon takes up.
        /// </summary>
        /// <returns>Integer width of icon.</returns>
        public int GetIconWidth()
        {
            return this.sourceIcon.Width;
        }

        /// <summary>
        /// Gets the height in cells that the icon takes up.
        /// </summary>
        /// <returns>Integer height of icon.</returns>
        public int GetIconHeight()
        {
            return this.sourceIcon.Height;
        }

        /// <summary>
        /// Formats the creature's stats into text to be displayed on the UI.
        /// </summary>
        /// <returns>[name,stats,actions,maxhp,curhp].</returns>
        public string[] GetStats()
        {
            string[] formattedStats = new string[5];

            formattedStats[0] = this.stats.Name;
            formattedStats[1] = this.stats.Stats;
            formattedStats[2] = this.stats.Actions;
            formattedStats[3] = this.stats.MaxHP.ToString();
            formattedStats[4] = this.stats.CurHP.ToString();

            return formattedStats;
        }

        /// <summary>
        /// Changes the stored current health of the creature.
        /// </summary>
        /// <param name="health">the new value to be used.</param>
        public void UpdateHealth(int health)
        {
            this.stats.CurHP = health;
        }

        /// <summary>
        /// Changes the stored name of the creature (does not change permanantly).
        /// </summary>
        /// <param name="name">the new name for the icon.</param>
        public void UpdateName(string name)
        {
            this.stats.Name = name;
        }

        /// <summary>
        ///  Ovverided method to draw the shape being displayed.
        /// </summary>
        /// <param name="drawingContext">The canvas on which to draw.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.selected)
            {
                Pen pen = new Pen(Brushes.Gold, 5);

                System.Windows.Rect ellipseBounds = new System.Windows.Rect(0, 0, this.RenderSize.Width, this.RenderSize.Height);

                drawingContext.DrawEllipse(this.fullIcon.Fill, pen, new System.Windows.Point(ellipseBounds.Width / 2, ellipseBounds.Height / 2), ellipseBounds.Width / 2, ellipseBounds.Height / 2);
            }
            else
            {
                Pen pen = new Pen(this.fullIcon.Stroke, this.fullIcon.StrokeThickness);

                System.Windows.Rect ellipseBounds = new System.Windows.Rect(0, 0, this.RenderSize.Width, this.RenderSize.Height);

                drawingContext.DrawEllipse(this.fullIcon.Fill, pen, new System.Windows.Point(ellipseBounds.Width / 2, ellipseBounds.Height / 2), ellipseBounds.Width / 2, ellipseBounds.Height / 2);
            }
        }
    }
}
