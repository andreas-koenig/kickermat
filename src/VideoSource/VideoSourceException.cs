using System;

namespace VideoSource
{
    public class VideoSourceException : Exception
    {
        public VideoSourceException() : base() { }

        public VideoSourceException(string message) : base(message) { }

        public VideoSourceException(string message, Exception ex) : base(message, ex) { }
    }
}
