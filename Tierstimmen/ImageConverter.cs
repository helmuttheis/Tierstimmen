using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Tierstimmen
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        static Dictionary<string, ImageSource> dictImageStreams = new Dictionary<string, ImageSource>();
        public static void Reset()
        {
            dictImageStreams = new Dictionary<string, ImageSource>();
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;

            Label lblName = parameter as Label;
            string szName = lblName.Text;

            if (value != null)
            {
                try
                {
                    if( !dictImageStreams.ContainsKey(szName))
                    {
                        byte[] imageAsBytes = (byte[])value;
                        
                        dictImageStreams.Add(szName, ImageSource.FromStream(() => new MemoryStream(imageAsBytes)));
                    }
                    retSource =  dictImageStreams[szName];
                }
                catch (Exception ex)
                {
                    Console.WriteLine("" + ex.ToString());
                }
            }
            return retSource;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
