namespace ImageProcessing.ImageMorphology
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using Utilities;

    /// <summary>
    /// Class for executing morphologic operations.
    /// </summary>
    public class EmguImageMorphology : IImageProcessor
    {
        /// <summary>
        /// The Settings used by the current instance for morphologic Settings.
        /// </summary>
        private readonly EmguImageMorphologySettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmguImageMorphology"/> class.
        /// </summary>
        /// <param name="settings">The Settings.</param>
        public EmguImageMorphology(EmguImageMorphologySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
            this.SettingsUserControl = new EmguImageMorphologyUserControl(settings);
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
            //var img = new Image<Rgb, byte>(sourceImage.Bitmap);
            return Execute(sourceImage, this.settings);
        }

        /// <summary>
        /// Executes a morphologic operation with the specified source image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="settings">The Settings.</param>
        /// <returns>The processed image. If no operation is executed, the sourceImage is returned.</returns>
        private IImage Execute(IImage sourceImage, EmguImageMorphologySettings settings)
        {
            if (sourceImage == null)
            {
                return null;
            }

            Image<Gray, byte> grayImage;
            if (sourceImage is Image<Gray, byte>)
                grayImage = sourceImage as Image<Gray, byte>;
            else
                grayImage = new Image<Gray, byte>(sourceImage.Bitmap);

            var resultimage = new Image<Gray, byte>(sourceImage.Size.Width, sourceImage.Size.Height);
            if (settings.NoiseReductionEnabled)
            {
                try
                {
                    Matrix<byte> kernel = new Matrix<byte>(new byte[,] {
                        {1,1,1 },
                        {1,1,1 },
                        {1,1,1 }
                    });
                    CvInvoke.MorphologyEx(grayImage, resultimage, Emgu.CV.CvEnum.MorphOp.Erode, kernel, new Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Default, CvInvoke.MorphologyDefaultBorderValue);
                    var temp = new Image<Gray, byte>(sourceImage.Size.Width, sourceImage.Size.Height);
                    CvInvoke.MorphologyEx(resultimage, temp, Emgu.CV.CvEnum.MorphOp.Dilate, kernel, new Point(-1, -1), 2, Emgu.CV.CvEnum.BorderType.Default, CvInvoke.MorphologyDefaultBorderValue);
                    grayImage = temp;
                }
                catch (Exception)
                {
                }
            }
            if (settings.PerformGaussianSmoothing)
                try
                {
                    var img = grayImage.SmoothGaussian(settings.SmoothKernelHeight, settings.SmoothKernelWidth, settings.SmoothSigma1, settings.SmoothSigma2);
                    grayImage = img;
                }
                catch (Exception)
                {
                }
            if (!settings.MorphologyEnabled)
                return grayImage;
            try
            {
                Matrix<byte> kernel = new Matrix<byte>(settings.MorphologyMaskWidth, settings.MorphologyMaskHeight);
                for (int i = 0; i < settings.MorphologyMaskWidth; i++)
                {
                    for (int j = 0; j < settings.MorphologyMaskHeight; j++)
                    {
                        kernel.Data[i, j] = 1;
                    }
                }
                CvInvoke.MorphologyEx(grayImage, resultimage, settings.PerformedMorphologicOperation, kernel, new Point(-1, -1), settings.MorphologyIterations, settings.BorderType, CvInvoke.MorphologyDefaultBorderValue);
            }
            catch (Exception)
            {
                return grayImage;
            }

            return resultimage;
        }
    }
}