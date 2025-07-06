// <copyright file="WidthToFontSizeConverter.cs" company="StaticSnap">
// Copyright (c) StaticSnap. All rights reserved.
// </copyright>

namespace TableTopHubApp
{
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Converter used by UI to match the size of text inside a box with the size of the box.
    /// </summary>
    public class WidthToFontSizeConverter : IMultiValueConverter
    {
        /// <summary>
        /// converts width of object to 1/10 for font size.
        /// </summary>
        /// <param name="values">the object containing the text.</param>
        /// <param name="targetType">extranious Type.</param>
        /// <param name="parameter">extranious object.</param>
        /// <param name="culture">extranious CultureInfo.</param>
        /// <returns>size font should be.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2 && values[0] is double width &&  values[1] is double height)
            {
                if(width * 1.2 < height)
                {
                    return width * 0.4;
                }
                else
                {
                    return height * 0.4;
                }
            }

            return 12.0; // Default font size
        }

        /// <summary>
        /// required by converter inherit.
        /// </summary>
        /// <param name="value">value.</param>
        /// <param name="targetType">targetType.</param>
        /// <param name="parameter">parameter.</param>
        /// <param name="culture">culture.</param>
        /// <returns>the value unchanged.</returns>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(); // No need to convert back
        }
    }
}
