using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SASpriteGen.Wpf.Converters
{
	public class FrameIndexToVisibilityConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return (int)values[0] == (int)values[1] ? Visibility.Visible : Visibility.Hidden;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
