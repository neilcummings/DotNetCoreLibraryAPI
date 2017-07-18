using System.Collections.Generic;

namespace WebApi.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public bool Revert { get; set; }

        public PropertyMappingValue(IEnumerable<string> desinationProperties, bool revert = false)
        {
            DestinationProperties = desinationProperties;
            Revert = revert;
        }
    }
}