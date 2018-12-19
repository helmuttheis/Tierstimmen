using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Tierstimmen
{
    public partial class TierstimmenLoadPage : ContentPage
    {
        MyBindingData bindingData = new MyBindingData();
        public TierstimmenLoadPage()
		{
           
            InitializeComponent();
            this.BindingContext = bindingData;
           // tbFilename.Text = bindingData.Filename;
           // tbUrl.Text = bindingData.Url;

        }

        async void OnLoadClicked(object sender, EventArgs e)
		{
            bindingData.Url = tbUrl.Text.Trim();
            bindingData.Filename = tbFilename.Text.Trim();
            HttpClient httpClient = new HttpClient();
            if( !bindingData.Url.EndsWith("/"))
            {
                bindingData.Url += "/";
            }
            SetStatus("verbinde mit " + bindingData.Url);
            if (bindingData.Filename.EndsWith(".JSON", StringComparison.InvariantCultureIgnoreCase))
            {
                string szJson = "";
                try
                {
                    szJson = await httpClient.GetStringAsync(bindingData.Url + bindingData.Filename);

                }
                catch (Exception ex)
                {
                    SetStatus("ERROR: " + ex.ToString());
                }

                SetStatus(szJson.Substring(0, 80));

                var ret = Newtonsoft.Json.Linq.JObject.Parse(szJson);
                JArray data = (JArray)ret.SelectToken("$.Data");

                int iCnt = data.Count;
                int iNr = 0;
                foreach (var item in data)
                {
                    iNr++;
                    TierstimmenItem tsItem = new TierstimmenItem();
                    Boolean bFehler = false;
                    foreach (JProperty property in item)
                    {

                        if (property.Name.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                        {
                            tsItem.Name = property.Value.ToString();

                            SetStatus(iNr.ToString() + " - " + iCnt.ToString() + " " + tsItem.Name);
                        }
                        else if (property.Name.Equals("Beschreibung", StringComparison.InvariantCultureIgnoreCase))
                        {
                            tsItem.Beschreibung = property.Value.ToString();
                        }
                        else if (property.Name.Equals("Gruppe", StringComparison.InvariantCultureIgnoreCase))
                        {
                            tsItem.Gruppe = property.Value.ToString();
                        }
                        else if (property.Name.Equals("Ton", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string tonUrl = property.Value.ToString();
                            try
                            {
                                tsItem.Ton = await httpClient.GetByteArrayAsync(bindingData.Url + tonUrl);
                            }
                            catch (Exception)
                            {
                                bFehler = true;
                            }
                        }
                        if (property.Name.Equals("Bild", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string bildUrl = property.Value.ToString();
                            try
                            {
                                tsItem.Bild = await httpClient.GetByteArrayAsync(bindingData.Url + bildUrl);
                            }
                            catch (Exception)
                            {
                                bFehler = true;
                            }
                        }

                    }
                    tsItem.Selected = false;
                    
                    if (!bFehler)
                    {
                        await App.Database.SaveItemAsync(tsItem);
                    }
                    else
                    {
                        // log error?
                    }
                }
            }
            else if (bindingData.Filename.EndsWith(".DB3", StringComparison.InvariantCultureIgnoreCase))
            {
                // TierstimmenSQLite.db3
                // App.Database.szFilename
                await App.Database.Close();
                try
                {
                    Stream strm = await httpClient.GetStreamAsync(bindingData.Url + bindingData.Filename);
                    if( strm != null)
                    {
                        FileStream fileStream = new FileStream(App.Database.szFilename, 
                            FileMode.Create, FileAccess.Write, FileShare.None);
                        
                        await  strm.CopyToAsync(fileStream).ContinueWith(
                            (copyTask) =>
                            {
                                fileStream.Close();
                            });
                    }
                }
                catch (Exception ex)
                {
                    SetStatus("ERROR: " + ex.ToString());
                }
                await App.Database.Reopen();
            }
        }

        
        void SetStatus(string str)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                lblStatus.Text = str;
            });
        }
	    void OnResetClicked(object sender, EventArgs e)
		{
            // recreate the databse
            App.Database.Reset();
  
		}

		async void OnCancelClicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync();
		}

		
    }
    public class MyBindingData: INotifyPropertyChanged
    {
        public string Url { get; set; } = "http://192.168.43.48:1337/unc/surfacehelle/ddrive/tmp/tierstimmen"; //"http://192.168.43.48:1337/unc/surfacehelle/ddrive/tmp/tierstimmen";
        public string Filename { get; set; } = "tsall.json";//"TierstimmenSQLite.db3";
        public string Status { get; set; } = "status";

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
