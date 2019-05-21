namespace Utilities
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using GlobalDataTypes;

    /// <summary>
    /// Universal utility class covering a variety of problems.
    /// </summary>
    public sealed class SwissKnife
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="SwissKnife"/> class from being created.
        /// </summary>
        private SwissKnife()
        {
        }

        /// <summary>
        /// Converts the spcified HLS color to RGB.
        /// </summary>
        /// <param name="hue">The hue value of the color.</param>
        /// <param name="lum">The luminescence value of the color.</param>
        /// <param name="sat">The saturation value of the color.</param>
        /// <returns>The corresponding RGB color.</returns>
        public static Color HLStoRGB(int hue, int lum, int sat)
        {
            double v;
            double r, g, b;
            double h, l, sl;

            if (hue < 0)
            {
                h = 0.0;
            }
            else if (hue > 255)
            {
                h = 1.0;
            }
            else
            {
                h = (double)hue / 255.0;
            }

            if (lum < 0)
            {
                l = 0.0;
            }
            else if (lum > 255)
            {
                l = 1.0;
            }
            else
            {
                l = (double)lum / 255.0;
            }

            if (sat < 0)
            {
                sl = 0.0;
            }
            else if (hue > 255)
            {
                sl = 1.0;
            }
            else
            {
                sl = (double)sat / 255.0;
            }

            // default to gray
            r = l;
            g = l;
            b = l;

            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - (l * sl));

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;

                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;

                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;

                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;

                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;

                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return Color.FromArgb(Convert.ToInt32(r * 255.0f), Convert.ToInt32(g * 255.0f), Convert.ToInt32(b * 255.0f));
        }

        /// <summary>
        /// Shows a modal message box for an exception.
        /// </summary>
        /// <param name="obj">The object where the exception occured (<c>this</c>).</param>
        /// <param name="ex">The exception to show.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowException(object obj, Exception ex)
        {
            return ShowException(obj, ex, false);
        }

        /// <summary>
        /// Shows a modal message box for an exception.
        /// </summary>
        /// <param name="obj">The object where the exception occured (<c>this</c>).</param>
        /// <param name="ex">The exception to show.</param>
        /// <param name="cancel">If a cancel button should be shown.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowException(object obj, Exception ex, bool cancel)
        {
            return ShowError(obj, ex.GetType().Name + ":\n\n" + ex.Message + "\n\n" + ex.StackTrace, cancel);
        }

        /// <summary>
        /// Shows a modal message box with an error message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The error message to show.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowError(object obj, string msg)
        {
            return ShowError(obj, msg, false);
        }

        /// <summary>
        /// Shows a modal message box with an error message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The error message to show.</param>
        /// <param name="cancel">If a cancel button should be shown.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowError(object obj, string msg, bool cancel)
        {
            MessageBoxButtons buttons = cancel ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK;
            return MsgBoxInvoker(msg, obj.GetType().Name + " Error", buttons, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a modal message box with a warning message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The warning message to show.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowWarning(object obj, string msg)
        {
            return ShowWarning(obj, msg, false);
        }

        /// <summary>
        /// Shows a modal message box with a warning message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The warning message to show.</param>
        /// <param name="cancel">If a cancel button should be shown.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowWarning(object obj, string msg, bool cancel)
        {
            MessageBoxButtons buttons = cancel ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK;
            return MsgBoxInvoker(msg, obj.GetType().Name + " Warning", buttons, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows a modal message box with an informational message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The message to show.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowInformation(object obj, string msg)
        {
            return ShowInformation(obj, msg, "Information", false);
        }

        /// <summary>
        /// Shows a modal message box with an informational message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The message to show.</param>
        /// <param name="title">Additinal string for the title of the message box.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowInformation(object obj, string msg, string title)
        {
            return ShowInformation(obj, msg, title, false);
        }

        /// <summary>
        /// Shows a modal message box with an informational message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The message to show.</param>
        /// <param name="cancel">If a cancel button should be shown.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowInformation(object obj, string msg, bool cancel)
        {
            return ShowInformation(obj, msg, "Information", cancel);
        }

        /// <summary>
        /// Shows a modal message box with an informational message.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="msg">The message to show.</param>
        /// <param name="title">Additinal string for the title of the message box.</param>
        /// <param name="cancel">If a cancel button should be shown.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowInformation(object obj, string msg, string title, bool cancel)
        {
            MessageBoxButtons buttons = cancel ? MessageBoxButtons.OKCancel : MessageBoxButtons.OK;
            return MsgBoxInvoker(msg, obj.GetType().Name + " " + title, buttons, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows a modal message box with a question for the user.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="question">The question to show.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowQuestion(object obj, string question)
        {
            return ShowQuestion(obj, question, "Question");
        }

        /// <summary>
        /// Shows a modal message box with a question for the user.
        /// </summary>
        /// <param name="obj">The object where the error occured (<c>this</c>).</param>
        /// <param name="question">The question to show.</param>
        /// <param name="title">Additinal string for the title of the message box.</param>
        /// <returns>The result of the message box.</returns>
        public static DialogResult ShowQuestion(object obj, string question, string title)
        {
            return MsgBoxInvoker(question, obj.GetType().Name + " " + title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Recalculates a coordinate value to correct the parallax error.
        /// </summary>
        /// <param name="longZ">The distance of the camera to the image plain.</param>
        /// <param name="shortZ">The distance of the projection plain above the image plain.</param>
        /// <param name="center">The base coordinate of the center of the plain.</param>
        /// <param name="value">The coordinate value to correct.</param>
        /// <returns>The corrected value.</returns>
        public static int ParallaxCorrection(int longZ, int shortZ, int center, int value)
        {
            if (longZ > 0 && shortZ > 0 && longZ > shortZ)
            {
                double factor = (double)(longZ - shortZ) / ((double)longZ);
                int cvalue = (int)Math.Round((double)(value - center) / factor);
                return center + cvalue;
            }

            return value;
        }

        /// <summary>
        /// Recalculates a position to correct the parallax error.
        /// </summary>
        /// <param name="longZ">The distance of the camera to the image plain.</param>
        /// <param name="shortZ">The distance of the projection plain above the image plain.</param>
        /// <param name="center">The center of the plain.</param>
        /// <param name="position">The position to correct.</param>
        public static void ParallaxCorrection(int longZ, int shortZ, Position center, Position position)
        {
            if (longZ > 0 && shortZ > 0 && longZ > shortZ)
            {
                double factor = (double)(longZ - shortZ) / ((double)longZ);
                int cx = (int)Math.Round((double)(position.XPosition - center.XPosition) / factor);
                int cy = (int)Math.Round((double)(position.YPosition - center.YPosition) / factor);
                position.XPosition = center.XPosition + cx;
                position.YPosition = center.YPosition + cy;
                var rect = position.BoundingBox;
                position.BoundingBox = new Rectangle(position.XPosition - position.BoundingBox.Width / 2, position.YPosition - position.BoundingBox.Height / 2, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// Actually shows a message box, modal if the main window handle is valid.
        /// </summary>
        /// <param name="text">The text for the message box.</param>
        /// <param name="title">The title of the message box.</param>
        /// <param name="buttons">The buttons on the message box.</param>
        /// <param name="icon">The icon on the message box.</param>
        /// <returns>The result of the message box.</returns>
        private static DialogResult MsgBoxInvoker(string text, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            try
            {
                return MessageBox.Show(Win32Window.MainWindow, text, title, buttons, icon, MessageBoxDefaultButton.Button1);
            }
#pragma warning disable 168
            catch (Win32Exception e)
#pragma warning restore 168
            {
                // Exception occurs if the window handle is invalid
                return MessageBox.Show(text, title, buttons, icon, MessageBoxDefaultButton.Button1);
            }
        }
    }
}