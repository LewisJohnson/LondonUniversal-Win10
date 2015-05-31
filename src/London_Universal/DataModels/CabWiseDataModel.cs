using System.Collections.Generic;

namespace London_Universal.DataModels
{

    public class CabWiseOperatorList
    {
        public int operatorId { get; set; }
        public string organisationName { get; set; }
        public string tradingName { get; set; }
        public List<object> alsoKnownAs { get; set; }
        public int centreId { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string town { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public string bookingsPhoneNumber { get; set; }
        public string bookingsEmail { get; set; }
        public bool publicAccess { get; set; }
        public bool publicWaitingRoom { get; set; }
        public bool wheelchairAccessible { get; set; }
        public bool creditDebitCard { get; set; }
        public bool chequeBankersCard { get; set; }
        public bool accountServicesAvailable { get; set; }
        public bool hoursOfOperation24X7 { get; set; }
        public bool hoursOfOperationMonThu { get; set; }
        public string startTimeMonThu { get; set; }
        public string endTimeMonThu { get; set; }
        public bool hoursOfOperationFri { get; set; }
        public string startTimeFri { get; set; }
        public string endTimeFri { get; set; }
        public bool hoursOfOperationSat { get; set; }
        public string startTimeSat { get; set; }
        public string endTimeSat { get; set; }
        public bool hoursOfOperationSun { get; set; }
        public string startTimeSun { get; set; }
        public string endTimeSun { get; set; }
        public bool hoursOfOperationPubHol { get; set; }
        public string startTimePubHol { get; set; }
        public string endTimePubHol { get; set; }
        public int numberOfVehicles { get; set; }
        public int numberOfVehiclesWheelchair { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public List<string> operatorTypes { get; set; }
        public double distance { get; set; }
    }

    public class Operators
    {

        public List<CabWiseOperatorList> operatorList { get; set; }
    }

    public class CabWiseAttribution
    {

        public string link { get; set; }
        public string text { get; set; }
        public string logo { get; set; }
    }

    public class CabWiseHeader
    {

        public string identifier { get; set; }
        public string displayTitle { get; set; }
        public string version { get; set; }
        public string publishDateTime { get; set; }
        public string canonicalPublishDateTime { get; set; }
        public string author { get; set; }
        public string owner { get; set; }
        public int refreshRate { get; set; }
        public int max_Latency { get; set; }
        public int timeToError { get; set; }
        public string schedule { get; set; }
        public string logo { get; set; }
        public string language { get; set; }
        public CabWiseAttribution CabWiseAttribution { get; set; }
    }

    public class CabWiseRootObject
    {

        public Operators operators { get; set; }
        public CabWiseHeader CabWiseHeader { get; set; }
    }

}
