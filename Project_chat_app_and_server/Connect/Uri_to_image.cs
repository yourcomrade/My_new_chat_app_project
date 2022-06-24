using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Project_chat_app_and_server.Connect
{
    public class Uri_to_image:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = value as string;
            if (!string.IsNullOrEmpty(url) &&!url.Equals(" "))
            {
                Uri uri = new Uri(url);
                return new BitmapImage(uri);
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}