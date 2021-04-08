using System;
using System.Globalization;
using System.Windows.Data;

namespace SASpriteGen.Wpf.Converters
{
	public class NegateBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var sourceVal = (bool)value;
			return !sourceVal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
