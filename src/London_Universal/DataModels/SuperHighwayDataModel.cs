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


        public string Type { get; set; }

        public object[][] Coordinates { get; set; }
    }


    public class SuperCycleRootObject
    {

        public string Id { get; set; }

        public string Label { get; set; }

        public SuperCycleGeography Geography { get; set; }

        public bool Segmented { get; set; }

    }

    public enum SuperCyleHighwayType
    {
        LineString,
        MultiLineString
    }

        
}
