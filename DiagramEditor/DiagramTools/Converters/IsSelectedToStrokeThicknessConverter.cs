using System;
using System.Globalization;
using System.Windows.Data;

namespace DrawToolsLib
{
	[ValueConversion(typeof (bool), typeof (double))]
	public class IsSelectedToStrokeThicknessConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
								object parameter, CultureInfo culture)
		{
			var selected = (bool) value;

			return selected ? 1 : 0;
		}

		public object ConvertBack(object value, Type targetType,
									object parameter, CultureInfo culture)
		{
			return new NotSupportedException(this.GetType().Name);
		}
	}
}
