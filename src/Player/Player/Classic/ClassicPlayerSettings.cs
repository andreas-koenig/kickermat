using Api;
using Api.Settings;
using Api.Settings.Parameter;

namespace Webapp.Player.Classic
{
    public class ClassicPlayerSettings : ISettings
    {
        public string Name => "ClassicPlayer";

        private double _test1;

        [NumberParameter("Test1", @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, 
            sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", 10, 0, 20, 1)]
        public double TestNumber
        {
            get => _test1;
            set
            {
                if (value < 10 || value > 15)
                {
                    throw new KickermatException("Test1 has to be in [10, 15]");
                }

                _test1 = value;
            }
        }

        [NumberParameter("Test2", "A simple test parameter", 10, 0, 20, 1)]
        public double TestNumber2 { get; set; }

        [EnumParameter("Strategy Variants", "Check out the strategies", typeof(Strategy))]
        public Strategy Strategy { get; set; }

        [BooleanParameter("Special Feature", "This special feature enables blabla.", true, false)]
        public bool TestFeature { get; set; }

        [ColorRangeParameter("Ball Color", "Lorem ipsum dolor sit amet", 0, 10, 10, 100, 100, 100)]
        public ColorRange BallColor { get; set; } = new ColorRange(0, 0, 0, 100, 100, 100);
    }

    public enum Strategy
    {
        Fast,
        Safe,
        Special,
    }
}
