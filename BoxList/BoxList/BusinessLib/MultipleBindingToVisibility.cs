using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace BoxList.BusinessLib
{
    class MultipleBindingToVisibility : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)values[0];
            string name = (string)values[1];
            if (isChecked && !string.IsNullOrWhiteSpace(name))
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
