namespace ImageProcessing.ImageBinarization
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using Utilities;

    /// <summary>
    /// Class which performs image binarization independent of the plane count.
    /// </summary>
    public class EmguImageBinarization : IImageProcessor
    {
        /// <summary>
        /// The Settings used by the instance.
        /// </summary>
        private readonly EmguImageBinarizationSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmguImageBinarization"/> class.
        /// </summary>
        /// <param name="planeDescription">The plane description.</param>
        /// <param name="settings">The Settings.</param>
        /// <param name="colorModell">The color modell.</param>
        public EmguImageBinarization(string[] planeDescription, EmguImageBinarizationSettings settings)
        {
            if (planeDescription == null)
            {
                throw new ArgumentNullException("planeDescription");
            }

            if (planeDescription.Length != 3)
            {
                throw new ArgumentException("planeDescription must have a length of 3");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            if ((settings.ColorMaxValues == null) || (settings.ColorMaxValues.Length != 3))
            {
                throw new ArgumentNullException("settings");
            }

            if ((settings.ColorMinValues == null) || (settings.ColorMinValues.Length != 3))
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;

            this.SettingsUserControl = new EmguImageBinarizationUserControl(planeDescription, settings);
        }

        /// <summary>
        /// Gets the Settings user control.
        /// </summary>
        /// <value>The Settings user control.</value>
        public UserControl SettingsUserControl { get; private set; }

        /// <summary>
        /// Executes the operation on the specified source image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <returns>The result image.</returns>
        public IImage Execute(IImage sourceImage)
        {
            if (sourceImage == null)
            {
                return sourceImage;
            }
            return this.Execute(sourceImage, this.settings.PlaneCount, this.settings.ColorMinValues, this.settings.ColorMaxValues, this.settings.BinarizePlane, this.settings.InvertPlane);
        }

        /// <summary>
        /// Executes the binarization on the specified source image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="planeCount">The plane count.</param>
        /// <param name="planeMinValues">The plane min values.</param>
        /// <param name="planeMaxValues">The plane max values.</param>
        /// <param name="binarizePlane">The binarize plane.</param>
        /// <returns>The result image.</returns>
        public IImage Execute(IImage sourceImage, int planeCount, int[] planeMinValues, int[] planeMaxValues, bool[] binarizePlane, bool[] invertPlane)
        {
            List<Image<Gray, byte>> binarizedImages = new List<Image<Gray, byte>>();
            Image<Rgb, byte> sourceRgbImage;
            if (sourceImage is Image<Rgb, Byte>)
                sourceRgbImage = sourceImage as Image<Rgb, byte>;
            else
                sourceRgbImage = new Image<Rgb, byte>(sourceImage.Bitmap);

            var channels = sourceRgbImage.Split();
            int planeIndex = 0;
            foreach (var channel in channels)
            {
                if (binarizePlane[planeIndex])
                {
                    Image<Gray, byte> binaryImg;
                    binaryImg = channel.InRange(new Gray(planeMinValues[planeIndex]), new Gray(planeMaxValues[planeIndex]));
                    if (invertPlane[planeIndex])
                        binaryImg = binaryImg.Not();
                    binarizedImages.Add(binaryImg);
                }
                planeIndex++;
            }
            // Logic AND with all binary images
            Image<Gray, byte> resultImage;
            switch (binarizedImages.Count)
            {
                case 0:
                    // no error, return no image (equivalent to a NULL pointer)
                    return null;
                case 1:
                    resultImage = binarizedImages[0].Clone();
                    break;
                case 2:
                    resultImage = binarizedImages[0].And(binarizedImages[1]);
                    break;
                case 3:
                    Image<Gray, byte> tempImage;
                    tempImage = binarizedImages[0].And(binarizedImages[1]);
                    resultImage = tempImage.And(binarizedImages[2]);
                    tempImage.Dispose();
                    break;
                default:
                    throw new NotSupportedException("Only 1, 2 or 3 planes are supported");
            }

            foreach (var img in binarizedImages)
            {
                img.Dispose();
            }
            return resultImage;
        }
    }
}