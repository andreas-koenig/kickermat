using System;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configuration
{
    public class Writable<T> : IWriteable<T>
        where T : class, new()
    {
        private static Action _onChange;
        private readonly IHostingEnvironment _environment;
        private readonly IOptionsMonitor<T> _options;
        private readonly string _section;
        private readonly string _file;

        public Writable(
            IHostingEnvironment environment,
            IOptionsMonitor<T> options,
            string sectionPath,
            string file)
        {
            _environment = environment;
            _options = options;
            _section = sectionPath;
            _file = file;

            _options.OnChange((settings, path) =>
            {
                Console.WriteLine(path);
            });
        }

        public object ValueObject => _options.CurrentValue;

        public T Value => _options?.CurrentValue;

        public T Get(string name) => _options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = _environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(_file);
            var physicalPath = fileInfo.PhysicalPath;

            if (!fileInfo.Exists)
            {
                File.CreateText(fileInfo.PhysicalPath).Close();
            }

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath))
                ?? new JObject();

            var sections = _section.Split(":");
            var sectionName = sections[sections.Length - 1];
            var sectionObject = jObject.TryGetValue(sectionName, out JToken section)
                    ? JsonConvert.DeserializeObject<T>(section.ToString())
                    : (Value ?? new T());

            applyChanges?.Invoke(sectionObject);

            var sectionPath = _section.Replace(":", ".");
            var changes = JObject.Parse(JsonConvert.SerializeObject(sectionObject));

            var sectionToken = jObject.SelectToken(sectionPath);
            if (sectionToken != null)
            {
                sectionToken.Replace(changes);
            }
            else
            {
                var root = (JToken)new JObject();
                var currentSection = root;
                foreach (var key in sectionPath.Split("."))
                {
                    currentSection[key] = new JObject();
                    currentSection = currentSection[key];
                }

                currentSection.Replace(changes);
                jObject.Merge(root);
            }

            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));

            // Notify subscribers about update
            _onChange?.Invoke();
        }

        public void Update(Action<object> applyChanges)
        {
            Update((Action<T>)applyChanges);
        }

        public void RegisterChangeListener(Action onChange)
        {
            _onChange += onChange;
        }
    }
}
