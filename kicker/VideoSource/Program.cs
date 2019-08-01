using System;
using System.Threading.Tasks;
using VideoSource.Dalsa;

namespace VideoSource
{
    class Program
    {
        static void Main(string[] args)
        {
            IVideoSource videoSource = new DalsaVideoSource();
            videoSource.StartAcquisition();
            Console.ReadLine();
            videoSource.StopAcquisition();
        }
    }
}
