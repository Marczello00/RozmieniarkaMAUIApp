using System.Globalization;

namespace RozmieniarkaApp.Converters
{
    internal class AvailableBanknotesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = "";
            _ = int.TryParse(value.ToString(), out int numberOfBanknotesAvailable);
            if (numberOfBanknotesAvailable >= 0 && numberOfBanknotesAvailable <= 30)
            {
                output = numberOfBanknotesAvailable.ToString();
            }
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
