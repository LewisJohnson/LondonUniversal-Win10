// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Global

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
