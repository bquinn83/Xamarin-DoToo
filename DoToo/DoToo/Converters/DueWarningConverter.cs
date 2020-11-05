using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using Xamarin.Forms;

namespace DoToo.Converters
{
    class DueWarningConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var daysUntilDue = ((DateTime)value - DateTime.Now).Days;
            if (daysUntilDue <= 2)
            {
                return (Color)Application.Current.Resources["DangerColor"];
            } else if (daysUntilDue <= 7)
            {
                return (Color)Application.Current.Resources["WarningColor"];
            } else
            {
                return (Color)Application.Current.Resources["NormalColor"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}