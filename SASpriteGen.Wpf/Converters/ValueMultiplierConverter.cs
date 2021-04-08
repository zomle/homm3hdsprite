using System;
using System.Globalization;
using System.Windows.Data;

namespace SASpriteGen.Wpf.Converters
{
	internal sealed class ValueMultiplierConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double tmpvalue = System.Convert.ToDouble(value);
			double multiplier = System.Convert.ToDouble(parameter);

			return tmpvalue * multiplier;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
