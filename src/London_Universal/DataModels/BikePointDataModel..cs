using System.Collections.Generic;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace London_Universal.DataModels
{
    public class BikePointAdditionalProperty
    {
        public string category { get; set; }
        public string key { get; set; }
        public string sourceSystemKey { get; set; }
        public string value { get; set; }
        public string modified { get; set; }
    }

    public class BikePointRootObject
    {
        public string id { get; set; }
        public string url { get; set; }
        public string commonName { get; set; }
        public string placeType { get; set; }
        public List<BikePointAdditionalProperty> additionalProperties { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
