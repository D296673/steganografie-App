using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Storage;

namespace steganografie_App.helper
{
    internal class FileHelper
    {
        public static async Task<StorageFile> PickImageFileAsync()
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");

            return await picker.PickSingleFileAsync();
        }

        public static async Task<byte[]> GetPixelDataFromImageAsync(StorageFile file)
        {
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                return pixelData.DetachPixelData();
            }
        }

        public static async Task<StorageFile> SaveImageWithModifiedPixelsAsync(byte[] pixels, StorageFile originalFile)
        {
            var savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("PNG Image", new string[] { ".png" });
            savePicker.SuggestedFileName = "SteganografieAfbeelding";

            StorageFile saveFile = await savePicker.PickSaveFileAsync();
            if (saveFile != null)
            {
                using (IRandomAccessStream stream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (IRandomAccessStream originalStream = await originalFile.OpenAsync(FileAccessMode.Read))
                    {
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(originalStream);
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                        encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode, decoder.PixelWidth, decoder.PixelHeight, decoder.DpiX, decoder.DpiY, pixels);
                        await encoder.FlushAsync();
                    }
                }
            }
            return saveFile;
        }
    }
}

