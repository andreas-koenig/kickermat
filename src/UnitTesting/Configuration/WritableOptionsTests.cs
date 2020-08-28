using System.IO;
using Api.Settings;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace UnitTesting.Configuration
{
    public class WritableOptionsTests
    {
        private const string ValidSettings = "{\"Kicker\":{\"Camera\":{\"Dalsa\":{\"CameraName\": \"Nano_1\",\"Brightness\":1.0}}}}";
        private const string EmptySettings = "{\"Kicker\":{\"Camera\":{\"Dalsa\":{}}}}";
        private const string EmptyPath = "{\"Kicker\":{}}";
        private const string EmptyObject = "{}";
        private const string EmptyFile = "";
        private const string ValidUpdatedSettings = "{\"Kicker\":{\"Camera\":{\"Dalsa\":{\"CameraName\": \"Nano_2\",\"Brightness\":1.0}}}}";
        private const string NewCameraName = "Nano_2";
        private const string Section = "Kicker:Camera:Dalsa";

        private ITestOutputHelper _output;

        public WritableOptionsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(ValidSettings)]
        [InlineData(EmptySettings)]
        [InlineData(EmptyPath)]
        [InlineData(EmptyObject)]
        [InlineData(EmptyFile)]
        public void Update_DifferentSettings_EqualsTextFile(string jsonData)
        {
            // Arrange
            var fileName = Path.GetTempFileName();
            File.WriteAllText(fileName, jsonData);

            var env = new HostingEnvironment()
            {
                ContentRootFileProvider = new PhysicalFileProvider(Path.GetTempPath()),
            };
            var options = new Writable<Settings>(
                env, null, Section, Path.GetFileName(fileName));

            // Act
            options.Update(settings =>
            {
                settings.CameraName = NewCameraName;
                settings.Brightness = 1.0;
            });

            // Assert
            var updatedSettings = File.ReadAllText(fileName);
            dynamic tmp1 = JsonConvert.DeserializeObject(updatedSettings);
            updatedSettings = JsonConvert.SerializeObject(tmp1, Formatting.Indented);

            dynamic tmp2 = JsonConvert.DeserializeObject(ValidUpdatedSettings);
            var formattedValidSettings = JsonConvert.SerializeObject(tmp2, Formatting.Indented);

            Assert.Equal(updatedSettings, formattedValidSettings);
        }

        private class Settings : ISettings
        {
            public string CameraName { get; set; }

            public double Brightness { get; set; }

            public string Name => "TestSettings";
        }
    }
}
