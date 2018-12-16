using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Tierstimmen
{
	public class TierstimmenListPageCS : ContentPage
	{
		ListView listView;

		public TierstimmenListPageCS()
		{
			Title = "Tierstimmen";

			var toolbarItem = new ToolbarItem
			{
				Text = "+",
				Icon = Device.RuntimePlatform == Device.iOS ? null : "plus.png"
			};
			toolbarItem.Clicked += async (sender, e) =>
			{
				await Navigation.PushAsync(new TierstimmenItemPageCS
				{
					BindingContext = new TierstimmenItem()
				});
			};
			ToolbarItems.Add(toolbarItem);

			listView = new ListView
			{
				Margin = new Thickness(20),
				ItemTemplate = new DataTemplate(() =>
				{
					var label = new Label
					{
						VerticalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.StartAndExpand
					};
					label.SetBinding(Label.TextProperty, "Name");

					var tick = new Image
					{
						Source = ImageSource.FromFile("check.png"),
						HorizontalOptions = LayoutOptions.End
					};
					
					var stackLayout = new StackLayout
					{
						Margin = new Thickness(20, 0, 0, 0),
						Orientation = StackOrientation.Horizontal,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						Children = { label, tick }
					};

					return new ViewCell { View = stackLayout };
				})
			};
			listView.ItemSelected += async (sender, e) =>
			{
                //((App)App.Current).ResumeAtTierstimmenId = (e.SelectedItem as TierstimmenItem).ID;
                //Debug.WriteLine("setting ResumeAtTierstimmenId = " + (e.SelectedItem as TierstimmenItem).ID);

                if (e.SelectedItem != null)
                {
                    await Navigation.PushAsync(new TierstimmenItemPageCS
                    {
                        BindingContext = e.SelectedItem as TierstimmenItem
                    });
                }
			};

			Content = listView;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			// Reset the 'resume' id, since we just want to re-start here
			((App)App.Current).ResumeAtTierstimmenId = -1;
			listView.ItemsSource = await App.Database.GetItemsAsync();
		}
	}
}
