using System;
using System.Globalization;
using System.Windows.Data;

namespace SASpriteGen.Wpf.Converters
{
	internal sealed class ScaledOffsetConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			double scale1 = System.Convert.ToDouble(values[0]);
			double scale2 = System.Convert.ToDouble(values[1]);
			double offset = System.Convert.ToDouble(values[2]);

			return offset*scale1*scale2;// * scale;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
