using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozmieniarkaApp.Converters
{
    internal class AvailableBanknotesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = "";
            int numberOfBanknotesAvailable = 0;
            int.TryParse(value.ToString(), out numberOfBanknotesAvailable);
            if(numberOfBanknotesAvailable >= 0 && numberOfBanknotesAvailable <= 30)
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
