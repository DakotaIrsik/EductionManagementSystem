using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SilverLeaf.Screener.Admin
{
    public class Config
    {
        private static Config _current;

        [JsonConstructor]
        public Config()
        {
        }

        public static Config Current
        {
            get
            {
                if (_current != null)
                {
                    return _current;
                }

                var type = typeof(Config).GetTypeInfo();
                var resource = type.Namespace + ".config.Development" + ".json";
                #if RELEASE
                        resource = type.Namespace + ".config.Production" + ".json";
                #endif

                using (var stream = type.Assembly.GetManifestResourceStream(resource))
                using (var reader = new StreamReader(stream))
                {
                    var file = reader.ReadToEnd();
                    _current = JsonConvert.DeserializeObject<Config>(file);
                }

                return _current;
            }
        }

        public string Environment { get; set; }

        public Api[] Apis { get; set; }

        public Api GetApi(string name)
        {
            return Apis?.FirstOrDefault(a => a.Name == name);
        }

        public class Api
        {
            public string Name { get; set; }

            public string BaseUrl { get; set; }

            public Header[] Headers { get; set; }

            public class Header
            {
                public string Key { get; set; }

                public string Value { get; set; }
            }
        }
    }
}
