using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using steganografie_App.helper;
using System.Threading.Tasks;

namespace steganografie_App
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void VerbergTekstInAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            if (TextInput.Text != string.Empty)
            {
                StorageFile file = await FileHelper.PickImageFileAsync();
                if (file != null)
                {
                    byte[] pixels = await FileHelper.GetPixelDataFromImageAsync(file);
                    if (pixels != null)
                    {
                        string tekst = TextInput.Text;
                        byte[] tekstBytes = SteganografieHelper.ConvertTextToByteArray(tekst);
                        pixels = SteganografieHelper.VerbergTekstInPixels(pixels, tekstBytes);

                        StorageFile saveFile = await FileHelper.SaveImageWithModifiedPixelsAsync(pixels, file);

                        if (saveFile != null)
                        {
                            await ToonAfbeelding(saveFile);
                        }
                    }
                }
            }
            else
            {
                OutputText.Text = "je moet eerst iets invullen";
            }
        }

        private async Task ToonAfbeelding(StorageFile file)
        {
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(fileStream);
                AfbeeldingWeergave.Source = bitmapImage; 
            }
        }


        private async void LeesVerborgenTekstUitAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await FileHelper.PickImageFileAsync();
            if (file != null)
            {
                byte[] pixels = await FileHelper.GetPixelDataFromImageAsync(file);
                if (pixels != null)
                {
                    string verborgenTekst = SteganografieHelper.LeesVerborgenTekstUitPixels(pixels);
                    OutputText.Text = "Verborgen boodschap: " + verborgenTekst;
                }
            }
        }
    }
}
