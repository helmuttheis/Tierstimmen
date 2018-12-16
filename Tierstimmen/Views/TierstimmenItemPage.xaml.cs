using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tierstimmen
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TierstimmenItemPage : ContentPage
	{
		public TierstimmenItemPage (ImageSource imageSoure)
		{
			InitializeComponent ();
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async  (s, e) => {
                // handle the tap
                await Navigation.PopAsync();
            };
            img.GestureRecognizers.Add(tapGestureRecognizer);
            img.Source = imageSoure;
            
        }
	}
}