using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace steganografie_App.helper
{
    internal class SteganografieHelper
    {
        public static byte[] VerbergTekstInPixels(byte[] pixels, byte[] tekstBytes)
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

        public static string LeesVerborgenTekstUitPixels(byte[] pixels)
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

        public static byte[] ConvertTextToByteArray(string tekst)
        {
            byte[] tekstBytes = Encoding.UTF8.GetBytes(tekst);
            return VoegEindMarkeringToe(tekstBytes);
        }

        private static byte[] VoegEindMarkeringToe(byte[] data)
        {
            byte[] metEindMarkering = new byte[data.Length + 1];
            Array.Copy(data, metEindMarkering, data.Length);
            metEindMarkering[metEindMarkering.Length - 1] = 0;
            return metEindMarkering;
        }
    }
}

