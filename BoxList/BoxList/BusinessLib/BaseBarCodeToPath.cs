using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BoxList.BusinessLib
{
    public class BaseBarCodeToPath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageName = (string)value;
            if (!string.IsNullOrWhiteSpace(imageName))
            {
                return Path.Combine(ConfigEntry.Instance.BaseBarCodeImagePath, $"{imageName}{ConfigEntry.Instance.BaseBarCodeImageExt}");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
