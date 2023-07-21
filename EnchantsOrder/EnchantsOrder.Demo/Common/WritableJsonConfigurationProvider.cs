using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace EnchantsOrder.Demo.Common
{
    internal class WritableJsonConfigurationProvider(JsonConfigurationSource source) : JsonConfigurationProvider(source)
    {
        public override void Set(string key, string value)
        {
            JObject jsonObject;
            string filePath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
            using (StreamReader file = new(filePath))
            using (JsonTextReader reader = new(file))
            {
                jsonObject = (JObject)JToken.ReadFrom(reader);
                jsonObject[key] = value;
            }
            using StreamWriter stream = new(filePath);
            using JsonTextWriter writer = new(stream);
            jsonObject.WriteTo(writer);
            base.Set(key, value);
        }
    }
}
