using System;
using System.Drawing;
using System.IO;

namespace ADServerDAL.Helpers
{
	/// <summary>
	/// Klasa pomocnicza obsługująca operacje na obrazkach
	/// </summary>
	public static class ImageProcesorHelper
	{
		public const int ThumbnailSize = 50;

		/// <summary>
		/// Klasa pomocniczna przechowująca wynik operacji na obrazkach
		/// </summary>
		public class ResizeImageResult
		{
			public byte[] ResizedImage { get; set; }

			public byte[] Thumbnail { get; set; }
		}

		private static bool ThumbnailCallback()
		{
			return false;
		}

		/// <summary>
		/// Metoda zmieniająca rozmiar obrazka
		/// </summary>
		/// <param name="width">Szerokość po zmianie</param>
		/// <param name="height">Wysokość po zmianie</param>
		/// <param name="imageBytes">Aktualny obrazek w postaci binarnej</param>
		/// <param name="createThumbnail">Czy utworzyć miniaturkę obrazka</param>
		public static ResizeImageResult ResizeImage(int width, int height, byte[] imageBytes, bool createThumbnail = true)
		{
			var resizeResult = new ResizeImageResult();

			// Utwórz obiekt Image z tablicy bajtów
			var img = Image.FromStream(new MemoryStream(imageBytes));

			// Utwórz bitmapę o nowych rozmiarach
			var b = new Bitmap(img, new Size(width, height));

			if (createThumbnail)
			{
				// Wygeneruj miniaturkę obrazka o zadnych rozmiarach
				var thumbnail = img.GetThumbnailImage(ThumbnailSize, ThumbnailSize, ThumbnailCallback, IntPtr.Zero);

				// Przekonwertuj miniaturkę obrazka do postaci binarnej
				var ms = new MemoryStream();
				thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
				resizeResult.Thumbnail = ms.ToArray();
			}

			// Przeskaluj oryginalny obrazek do nowych rozmiarów
			var converter = new ImageConverter();
			imageBytes = (byte[])converter.ConvertTo(b, typeof(byte[]));
			resizeResult.ResizedImage = imageBytes;

			return resizeResult;
		}
	}
}