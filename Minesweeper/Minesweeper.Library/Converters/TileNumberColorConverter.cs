using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Minesweeper.Library.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class TileNumberColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int mines;
            if (!int.TryParse(value.ToString(), out mines))
                return null;

            Brush color;
            switch (mines)
            {
                case 1:
                    color = Brushes.Blue;
                    break;
                case 2:
                    color = Brushes.Green;
                    break;
                case 3:
                    color = Brushes.Red;
                    break;
                case 4:
                    color = Brushes.Purple;
                    break;
                case 5:
                    color = Brushes.Maroon;
                    break;
                case 6:
                    color = Brushes.Turquoise;
                    break;
                case 7:
                    color = Brushes.Black;
                    break;
                case 8:
                    color = Brushes.Gray;
                    break;

                default:
                    color = Brushes.White;
                    break;
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
