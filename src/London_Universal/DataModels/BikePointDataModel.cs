using System.Collections.Generic;


namespace London_Universal.DataModels
{
    public class BikePointAdditionalProperty
    {
        public string category { get; set; }
        public string key { get; set; }
        public string sourceSystemKey { get; set; }
        public string value { get; set; }
    }

    public class BikePointRootObject
    {
        public string url { get; set; }
        public string commonName { get; set; }
        public List<BikePointAdditionalProperty> additionalProperties { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
