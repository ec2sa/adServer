using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ADServerDAL.Helpers;
using System.Drawing;
using System.IO;

namespace AdServerUnitTests
{

    /// <summary>
    /// Grupa testów weryfikujących algorytmy do modyfikacji obrazków
    /// </summary>
    [TestClass]
    public class ImageResizeUnitTests
    {
        /// <summary>
        /// Sprawdzenie algorytmów do zmiany rozmiarów obrazków
        /// </summary>
        [TestMethod]
        public void Can_Resize_Images()
        {
            ImageProcesorHelper.ResizeImageResult resizeResult = null;
            Image newImage = null;

            /// Test zmiany rozmiaru obrazka na 500x500 bez miniaturki
            var bmp1 = ImageToByte(Properties.Resources.Chrysanthemum);
            resizeResult = ImageProcesorHelper.ResizeImage(500, 500, bmp1, false);
            Assert.IsNotNull(resizeResult);
            Assert.IsNotNull(resizeResult.ResizedImage);
            Assert.IsNull(resizeResult.Thumbnail);
            newImage = ByteArrayToImage(resizeResult.ResizedImage);
            Assert.AreEqual(500, newImage.Width);
            Assert.AreEqual(500, newImage.Height);

            ///Test zmiany obrazka na rozmiar taki sam jaki ma obrazek oryginalny bez miniaturki
            var bmp2 = Properties.Resources.Desert;
            resizeResult = ImageProcesorHelper.ResizeImage(bmp2.Width, bmp2.Height, ImageToByte(bmp2), false);
            Assert.IsNotNull(resizeResult);
            Assert.IsNotNull(resizeResult.ResizedImage);
            Assert.IsNull(resizeResult.Thumbnail);
            newImage = ByteArrayToImage(resizeResult.ResizedImage);
            Assert.AreEqual(bmp2.Width, newImage.Width);
            Assert.AreEqual(bmp2.Height, newImage.Height);

            ///Test wygenerowania miniaturki
            var bmp3 = Properties.Resources.Hydrangeas;
            resizeResult = ImageProcesorHelper.ResizeImage(bmp3.Width, bmp3.Height, ImageToByte(bmp3), true);
            Assert.IsNotNull(resizeResult);
            Assert.IsNotNull(resizeResult.ResizedImage);
            Assert.IsNotNull(resizeResult.Thumbnail);
            newImage = ByteArrayToImage(resizeResult.Thumbnail);
            Assert.IsTrue(newImage.Width == ImageProcesorHelper.ThumbnailSize && newImage.Height == ImageProcesorHelper.ThumbnailSize);
        }

        /// <summary>
        /// Metoda pomocniczna do konwersji obrazka do tablicy bajtów
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        /// <summary>
        /// Metoda pomocniczna do konwersji tablicy bajtów do obrazka
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
