using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace iDental.ViewModels.ViewModelBase
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool InvertVisibility { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {
                var visible = System.Convert.ToBoolean(value, culture);
                if (InvertVisibility)
                    visible = !visible;
                return visible ? Visibility.Visible : Visibility.Collapsed;
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
