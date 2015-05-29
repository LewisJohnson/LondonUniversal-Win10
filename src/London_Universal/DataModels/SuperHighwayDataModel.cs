using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace London_Universal.DataModels
{
    public class SuperCycleGeography
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("coordinates")]
        public object[][] Coordinates { get; set; }
    }


    public class SuperCycleRootObject
    {

        [JsonProperty("$id")]
        public string Id { get; set; }

        [JsonProperty("$type")]
        public string Type { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("geography")]
        public SuperCycleGeography Geography { get; set; }

        [JsonProperty("segmented")]
        public bool Segmented { get; set; }

    }
}
