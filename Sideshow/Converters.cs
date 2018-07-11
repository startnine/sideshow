using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Sideshow
{
    public class DoubleComparisonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            double val = double.Parse(value.ToString());
            int paramFirstNumerical = 0;
            for (int i = 0; i < parameter.ToString().Length; i++)
            {
                if (char.IsNumber(parameter.ToString().ElementAt(i)))
                {
                    paramFirstNumerical = i;
                    break;
                }
            }

            double param = double.Parse(parameter.ToString().Substring(paramFirstNumerical));

            string opr = parameter.ToString().Substring(0, paramFirstNumerical);

            if (opr == ">")
                return val > param;
            else if (opr == "<")
                return val < param;
            else if (opr == ">=")
                return val >= param;
            else if (opr == "<=")
                return val <= param;
            else
                return val >= param;
        }

        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FrameworkElementOrientationToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            FrameworkElement element = value as FrameworkElement;

            var returnVal = element.ActualWidth >= element.ActualHeight;
            Debug.WriteLine("ElementOrientation: " + returnVal.ToString());
            return returnVal;
        }

        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
