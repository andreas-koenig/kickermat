using System;
using System.Collections.Generic;
using System.Text;
using Api.Camera;
using OpenCvSharp;

namespace Video
{
    public static class OpenCvMatExtensions
    {
        private static readonly ImageEncodingParam _jpgQuality
            = new ImageEncodingParam(ImwriteFlags.JpegQuality, 75);

        public static byte[] ToJpg(this Mat image)
        {
            return image.ImEncode(".jpg", _jpgQuality);
        }
    }
}
