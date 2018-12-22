using PCLStorage;
using PCLStorage.Exceptions;
using Plugin.SimpleAudioPlayer;
using System;

using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tierstimmen
{
    public enum TSGRUPPE
    {
        VOEGEL,
        SAEUGER,
        INSEKTEN,
        AMPHIBIEN
    }
    public partial class TierstimmenListPage : ContentPage
    {
        private TSGRUPPE tsgruppe;

        public TierstimmenListPage(TSGRUPPE tsgruppe)
        {
            ByteArrayToImageSourceConverter.Reset();
            this.tsgruppe = tsgruppe;
            App.Database.szGruppe = tsgruppe.ToString().ToLower();
            App.Database.bUseSelected = false;

            InitializeComponent();

            this.Title += " " + App.Database.szGruppe.Substring(0, 1).ToUpper() + App.Database.szGruppe.Substring(1).ToLower();
        }
        private async void TapImage_Tapped(object sender, EventArgs e)
        {
            Image img = (Image)sender;
            await Navigation.PushAsync(new TierstimmenItemPage( img.Source));
        }
        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            listView.BeginRefresh();
            int iCount = 0;
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                listView.ItemsSource = await App.Database.GetItemsAsync();
                iCount = await App.Database.GetCountAsync("");
            }
            else
            {
                listView.ItemsSource = await App.Database.GetItemsAsync(e.NewTextValue);
                iCount = await App.Database.GetCountAsync(e.NewTextValue);
            }
            lblCount.Text = iCount.ToString() + " Einträge gefunden";
            if (iCount > App.Database.iLimitResults)
            {
                lblCount.Text += ", nur " + App.Database.iLimitResults.ToString() + " werden angezeigt.";
            }
            listView.EndRefresh();
        }
        private async void Search()
        {
            listView.BeginRefresh();
            int iCount = 0;
            if (string.IsNullOrWhiteSpace(sbSearch.Text))
            {
                listView.ItemsSource = await App.Database.GetItemsAsync();
                iCount = await App.Database.GetCountAsync("");
            }
            else
            {
                listView.ItemsSource = await App.Database.GetItemsAsync(sbSearch.Text);
                iCount = await App.Database.GetCountAsync(sbSearch.Text);
            }
            lblCount.Text = iCount.ToString() + " Einträge gefunden";
            if (iCount > App.Database.iLimitResults)
            {
                lblCount.Text += ", nur " + App.Database.iLimitResults.ToString() + " werden angezeigt.";
            }
            listView.EndRefresh();
        }
        private void SwUseSelected_Toggled(object sender, ToggledEventArgs e)
        {
            App.Database.bUseSelected = ((Switch)sender).IsToggled;
            Search();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Reset the 'resume' id, since we just want to re-start here
            // ((App)App.Current).ResumeAtTierstimmenId = -1;
            var objSelected = listView.SelectedItem;
            if (objSelected == null)
            {
                listView.ItemsSource = await App.Database.GetItemsAsync();
            }
            listView.SelectedItem = objSelected;
          //  Task.Run( () =>
          //  {
          //      Task.Delay(1500).Wait();
          //      Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
          //      {
          //          listView.ScrollTo(objSelected, ScrollToPosition.Center, true);
          //      });
          //  });
            
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Console.WriteLine("ListView_ItemTapped");
        }

        void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine("OnListItemSelected");
            //((App)App.Current).ResumeAtTierstimmenId = (e.SelectedItem as TierstimmenItem).ID;
            //Debug.WriteLine("setting ResumeAtTierstimmenId = " + (e.SelectedItem as TierstimmenItem).ID);
            if (e.SelectedItem != null)
            {
                // await Navigation.PushAsync(new TierstimmenItemPage
                // {
                //     BindingContext = e.SelectedItem as TierstimmenItem
                // });
            }
        }

        private void BtnPlay_Clicked(object sender, EventArgs e)
        {
            Console.WriteLine("BtnPlay_Clicked");
            //if (listView.SelectedItem == null)
            {
                ImageButton btnThis = (ImageButton)sender;

                var listItem = btnThis.BindingContext as TierstimmenItem;
                listView.SelectedItem = listItem;
            }
            // get the ton of the current item
            if (listView.SelectedItem != null)
            {
                var ts = listView.SelectedItem as TierstimmenItem;
                Play(ts.Ton);
            }
        }
        private int iToggle = 0;
        private void Play(byte[] audioBytes)
        {
            ISimpleAudioPlayer audioPlayer = CrossSimpleAudioPlayer.Current;
            audioPlayer.Stop();
            audioPlayer.Load(new MemoryStream(audioBytes));

            audioPlayer.Play();
            // IFolder folder = FileSystem.Current.LocalStorage;
            // for (int i=0;i<iToggle;i++)
            // {
            //     String tmpFilename = "sample" + i.ToString() + ".mp3";
            //     
            //     try
            //     {
            //         IFile tmpFile = await folder.GetFileAsync(tmpFilename);
            //         System.Diagnostics.Debug.WriteLine("delete " + tmpFilename);
            //         await tmpFile.DeleteAsync();
            //     }
            //     catch (FileNotFoundException ex)
            //     {
            //         System.Diagnostics.Debug.WriteLine("delete " + tmpFilename + " " + ex.ToString());
            //     }
            //     catch (Exception ex)
            //     {
            //         System.Diagnostics.Debug.WriteLine("delete " + tmpFilename + " " + ex.ToString());
            //     }
            // }
            // String filename = "sample" + iToggle.ToString() + ".mp3";
            // iToggle++;
            // 
            // IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            // 
            // using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
            // {
            //     stream.Write(audioBytes, 0, audioBytes.Length);
            // }
            // 
            // await CrossMediaManager.Current.Play("file://" + file.Path);

        }

        private void BtnStop_Clicked(object sender, EventArgs e)
        {
            // await CrossMediaManager.Current.Stop();
            ISimpleAudioPlayer audioPlayer = CrossSimpleAudioPlayer.Current;
            audioPlayer.Stop();

        }

        private void SwSelected_Toggled(object sender, ToggledEventArgs e)
        {
            Switch swThis = (Switch)sender;

            var listItem = swThis.BindingContext as TierstimmenItem;
            listView.SelectedItem = listItem;

            // get the ton of the current item
            if (listView.SelectedItem != null)
            {
                var ts = listView.SelectedItem as TierstimmenItem;
                ts.Selected = swThis.IsToggled;
                App.Database.SaveItemAsync(ts);
            }
        }

       
    }
}
