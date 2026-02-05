using System.Globalization;

namespace ListEngineCorsaLite.Converters;

public class FavoritoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isFavorito)
            return isFavorito ? "★" : "☆";
        return "☆";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string star)
            return star == "★";
        return false;
    }
}
