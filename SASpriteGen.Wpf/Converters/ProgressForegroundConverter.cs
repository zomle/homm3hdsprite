using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SASpriteGen.Wpf.Converters
{
	public class ProgressForegroundConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var inprogress = (bool)value;
			if (inprogress)
			{
				return Brushes.DodgerBlue;
			}
			else
			{
				return Brushes.LimeGreen;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
