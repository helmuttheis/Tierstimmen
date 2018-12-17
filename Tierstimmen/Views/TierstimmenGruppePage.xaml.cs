﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tierstimmen
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TierstimmenGruppePage : ContentPage
	{
		public TierstimmenGruppePage ()
		{
			InitializeComponent ();
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                // handle the tap
                Image img = (Image)s;
                if( img.Id == imgVoegel.Id )
                {
                    await Navigation.PushAsync(new TierstimmenListPage(TSGRUPPE.VOEGEL));
                }
                else if (img.Id == imgSaeuger.Id)
                {
                    await Navigation.PushAsync(new TierstimmenListPage(TSGRUPPE.SAEUGER));
                }
                else if (img.Id == imgAmphibien.Id)
                {
                    await Navigation.PushAsync(new TierstimmenListPage(TSGRUPPE.AMPHIBIEN));
                }
                else if (img.Id == imgInsekten.Id)
                {
                    await Navigation.PushAsync(new TierstimmenListPage(TSGRUPPE.INSEKTEN));
                }
            };
            imgAmphibien.GestureRecognizers.Add(tapGestureRecognizer);
            imgVoegel.GestureRecognizers.Add(tapGestureRecognizer);
            imgSaeuger.GestureRecognizers.Add(tapGestureRecognizer);
            imgInsekten.GestureRecognizers.Add(tapGestureRecognizer);

        }
         
        async void OnItemAdded(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TierstimmenLoadPage
            {
                // BindingContext = new TierstimmenItem()
            });
        }
    }
}