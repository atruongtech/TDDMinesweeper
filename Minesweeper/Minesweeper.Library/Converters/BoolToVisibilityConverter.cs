using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Minesweeper.Library.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public enum Inversion
        {
            Inverted,
            Normal
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool vis = (bool)value;
            var invert = (Inversion)Enum.Parse(typeof(Inversion), (string)parameter);
            if (invert == Inversion.Inverted)
            {
                if (vis)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
            if (vis)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
            

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
