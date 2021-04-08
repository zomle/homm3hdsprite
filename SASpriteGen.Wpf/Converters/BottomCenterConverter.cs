using System;
using System.Globalization;
using System.Windows.Data;

namespace SASpriteGen.Wpf.Converters
{ 
	internal sealed class BottomCenterConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			double scaleX = System.Convert.ToDouble(values[0]);
			double scaleY = System.Convert.ToDouble(values[1]);
			double canvasWidth = System.Convert.ToDouble(values[2]);
			double canvasHeight = System.Convert.ToDouble(values[3]);
			double controlWidth = System.Convert.ToDouble(values[4]) * scaleX;
			double controlHeight = System.Convert.ToDouble(values[5]) * scaleY;

			return (string)parameter switch
			{
				"top" => (canvasHeight - controlHeight)/2,
				"bottom" => (canvasHeight + controlHeight) /2,
				"left" => (canvasWidth - controlWidth) / 2,
				"right" => (canvasWidth + controlWidth) / 2,
				_ => 0,
			};
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
