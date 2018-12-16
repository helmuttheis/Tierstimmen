using System;
using System.IO;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Tierstimmen
{
	public class App : Application
	{
		private static TierstimmenItemDatabase database;

		public App()
		{
			Resources = new ResourceDictionary();
			Resources.Add("primaryGreen", Color.FromHex("91CA47"));
			Resources.Add("primaryDarkGreen", Color.FromHex("6FA22E"));

			var nav = new NavigationPage(new TierstimmenGruppePage());
			nav.BarBackgroundColor = (Color)App.Current.Resources["primaryGreen"];
			nav.BarTextColor = Color.White;

			MainPage = nav;
		}

		public static TierstimmenItemDatabase Database
		{
			get
			{
				if (database == null)
				{
                    database = new TierstimmenItemDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TierstimmenSQLite.db3"));
				}
				return database;
			}
		}

		public int ResumeAtTierstimmenId { get; set; }

		protected override void OnStart()
		{
			//Debug.WriteLine("OnStart");

			//// always re-set when the app starts
			//// users expect this (usually)
			////			Properties ["ResumeAtTierstimmenId"] = "";
			//if (Properties.ContainsKey("ResumeAtTierstimmenId"))
			//{
			//	var rati = Properties["ResumeAtTierstimmenId"].ToString();
			//	Debug.WriteLine("   rati=" + rati);
			//	if (!String.IsNullOrEmpty(rati))
			//	{
			//		Debug.WriteLine("   rati=" + rati);
			//		ResumeAtTierstimmenId = int.Parse(rati);

			//		if (ResumeAtTierstimmenId >= 0)
			//		{
			//			var todoPage = new TierstimmenItemPage();
			//			todoPage.BindingContext = await Database.GetItemAsync(ResumeAtTierstimmenId);
			//			await MainPage.Navigation.PushAsync(todoPage, false); // no animation
			//		}
			//	}
			//}
		}

		protected override void OnSleep()
		{
			//Debug.WriteLine("OnSleep saving ResumeAtTierstimmenId = " + ResumeAtTierstimmenId);
			//// the app should keep updating this value, to
			//// keep the "state" in case of a sleep/resume
			//Properties["ResumeAtTierstimmenId"] = ResumeAtTierstimmenId;
		}

		protected override void OnResume()
		{
			//Debug.WriteLine("OnResume");
			//if (Properties.ContainsKey("ResumeAtTierstimmenId"))
			//{
			//	var rati = Properties["ResumeAtTierstimmenId"].ToString();
			//	Debug.WriteLine("   rati=" + rati);
			//	if (!String.IsNullOrEmpty(rati))
			//	{
			//		Debug.WriteLine("   rati=" + rati);
			//		ResumeAtTierstimmenId = int.Parse(rati);

			//		if (ResumeAtTierstimmenId >= 0)
			//		{
			//			var todoPage = new TierstimmenItemPage();
			//			todoPage.BindingContext = await Database.GetItemAsync(ResumeAtTierstimmenId);
			//			await MainPage.Navigation.PushAsync(todoPage, false); // no animation
			//		}
			//	}
			//}
		}
	}
}

