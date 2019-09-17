using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configuration
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostingEnvironment _environment;
        private readonly IOptionsMonitor<T> _options;
        private readonly string _section;
        private readonly string _file;

        public WritableOptions(
            IHostingEnvironment environment,
            IOptionsMonitor<T> options,
            string sectionPath,
            string file)
        {
            _environment = environment;
            _options = options;
            _section = sectionPath;
            _file = file;
        }

        public object ValueObject => _options.CurrentValue;
        public T Value => _options.CurrentValue;
        public T Get(string name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = _environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(_file);
            var physicalPath = fileInfo.PhysicalPath;
            
            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            
            var sections = _section.Split(":");
            var sectionName = sections[sections.Length - 1];
            var sectionObject = jObject.TryGetValue(sectionName, out JToken section)
                ? JsonConvert.DeserializeObject<T>(section.ToString())
                : (Value ?? new T());

            applyChanges(sectionObject);

            var sectionPath = _section.Replace(":", ".");
            var changes = JObject.Parse(JsonConvert.SerializeObject(sectionObject));

            var sectionToken = jObject.SelectToken(sectionPath);
            if (sectionToken != null)
                sectionToken.Replace(changes);
            else
                jObject.AddFirst(changes);
            // TODO: Add new value at correct path if it does not exist yet.

            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        }

        public void Update(Action<object> applyChanges)
        {
            Update((Action<T>)applyChanges);
        }
    }
}
