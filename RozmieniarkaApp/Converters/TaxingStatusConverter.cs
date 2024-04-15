using System.Globalization;

namespace RozmieniarkaApp.Converters
{
    internal class TaxingStatusConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            _ = int.TryParse(value.ToString(), out int taxingStatus);
            if (taxingStatus == 1)
            {
                return "Status fiskalizacja włączona";
            }
            else if (taxingStatus == 0)
            {
                return "Status fiskalizacja wyłączona";
            }
            else if (taxingStatus == -1)
            {
                return "Status fiskalizacja nieznana";
            }
            else
            return "lol";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
