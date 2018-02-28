using System;
using System.Globalization;
using System.Windows.Data;

namespace iDental.ViewModels.ViewModelBase
{
    internal sealed class CenterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double canvasWidth = Math.Round(System.Convert.ToDouble(values[0]), 2);
            double canvasHeight = Math.Round(System.Convert.ToDouble(values[1]), 2);
            double controlWidth = Math.Round(System.Convert.ToDouble(values[2]), 2);
            double controlHeight = Math.Round(System.Convert.ToDouble(values[3]), 2);
            switch ((string)parameter)
            {
                case "top":
                    return (canvasHeight - controlHeight) / 2;
                case "bottom":
                    return (canvasHeight + controlHeight) / 2;
                case "left":
                    return (canvasWidth - controlWidth) / 2;
                case "right":
                    return (canvasWidth + controlWidth) / 2;
                default:
                    return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
