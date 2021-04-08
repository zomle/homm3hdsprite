using SASpriteGen.ViewModel;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SASpriteGen.Wpf.Converters
{
	public class TabEnumToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var parameterEnum = Enum.Parse<ActiveTab>((string)parameter);
			var valueEnum = (ActiveTab)value;

			return parameterEnum == valueEnum;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var parameterEnum = Enum.Parse<ActiveTab>((string)parameter);
			return parameterEnum;
		}
	}
}
