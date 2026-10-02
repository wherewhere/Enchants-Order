using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace EnchantsOrder.Demo.Common
{
    internal sealed class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new WritableJsonConfigurationProvider(this);
        }
    }
}
