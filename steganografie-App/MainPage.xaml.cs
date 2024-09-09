using System;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Popups;
using Windows.Graphics.Imaging;
using System.IO;
using Windows.UI.Xaml;

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

                    var picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".jpg");

                StorageFile file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                
                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                            PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                            byte[] pixels = pixelData.DetachPixelData();

                            string tekst = TextInput.Text;
                            byte[] tekstBytes = Encoding.UTF8.GetBytes(tekst);
                            tekstBytes = VoegEindMarkeringToe(tekstBytes);

                            pixels = VerbergTekstInPixels(pixels, tekstBytes);

                            var savePicker = new FileSavePicker();
                            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                            savePicker.FileTypeChoices.Add("PNG Image", new string[] { ".png" });
                            savePicker.SuggestedFileName = "SteganografieAfbeelding";

                            StorageFile saveFile = await savePicker.PickSaveFileAsync();
                            if (saveFile != null)
                            {
                                using (IRandomAccessStream stream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
                                {
                                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                                    encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode, decoder.PixelWidth, decoder.PixelHeight, decoder.DpiX, decoder.DpiY, pixels);
                                    await encoder.FlushAsync();
                                }
                            }
                        }
                    }
                    
                }
            else
            {
                OutputText.Text = "je moet eerst iets invullen";
            }
        }

        private async void LeesVerborgenTekstUitAfbeelding_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                    PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                    byte[] pixels = pixelData.DetachPixelData();

                    string verborgenTekst = LeesVerborgenTekstUitPixels(pixels);

                    OutputText.Text = "Verborgen boodschap: " + verborgenTekst;
                }
            }
        }

        private byte[] VerbergTekstInPixels(byte[] pixels, byte[] tekstBytes)
        {
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < pixels.Length; i += 4) 
            {
                pixels[i + 2] &= 0xFE; 

                if (byteIndex < tekstBytes.Length)
                {
                    pixels[i + 2] |= (byte)((tekstBytes[byteIndex] >> bitIndex) & 1);

                    bitIndex++;

                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        byteIndex++;
                    }
                }
            }

            return pixels;
        }

        private string LeesVerborgenTekstUitPixels(byte[] pixels)
        {
            StringBuilder resultaat = new StringBuilder();
            int byteValue = 0, bitIndex = 0;

            for (int i = 0; i < pixels.Length; i += 4) 
            {
                int blauw = pixels[i + 2] & 1; 

                byteValue |= blauw << bitIndex;
                bitIndex++;

                if (bitIndex == 8)
                {
                    char character = (char)byteValue;
                    if (character == '\0') 
                        break;

                    resultaat.Append(character);
                    byteValue = 0;
                    bitIndex = 0;
                }
            }

            return resultaat.ToString();
        }

        private byte[] VoegEindMarkeringToe(byte[] data)
        {
            byte[] metEindMarkering = new byte[data.Length + 1];
            Array.Copy(data, metEindMarkering, data.Length);
            metEindMarkering[metEindMarkering.Length - 1] = 0; 
            return metEindMarkering;
        }
    }
}

