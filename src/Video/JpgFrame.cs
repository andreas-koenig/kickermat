using System;
using System.Collections.Generic;
using System.Text;
using Api.Camera;
using OpenCvSharp;

namespace Video
{
    public class JpgFrame : IFrame
    {
        public JpgFrame(byte[] jpgImage)
        {
            Image = jpgImage;
        }

        public JpgFrame(Mat frame)
        {
            Image = frame.ToJpg();
        }

        public byte[] Image { get; protected set; }

        public object Clone()
        {
            return new JpgFrame(Image.Clone() as byte[]);
        }

        public byte[] ToBytes()
        {
            return Image.Clone() as byte[];
        }
    }
}
