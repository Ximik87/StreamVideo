using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Streaming.WpfApp
{
    public class BitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stream = value as Stream;
            stream.Seek(0, SeekOrigin.Begin);
            var bitMap = new BitmapImage();
            bitMap.BeginInit();
            bitMap.StreamSource = stream;
            bitMap.EndInit();

            return bitMap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
