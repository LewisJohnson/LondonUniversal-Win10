namespace London_Universal.Models
{
    static class Constants
    {
        private const string AppId = "4238c1bf";
        private const string AppKey = "eaf40b6c793133ac35d48fc1ec6eccdd";

        public static string BikeAllPoints()
        {
            const string baseS = "http://api.tfl.gov.uk/BikePoint?app_id={0}&app_key={1}";
            return string.Format(baseS, AppId, AppKey);
        }

        public static string BikeSearchByIdTask(string point)
        {
            const string baseS = "http://api.tfl.gov.uk/BikePoint/%7Bids%7D?ids={2}&app_id={0}&app_key={1}";
            return string.Format(baseS, AppId, AppKey, point);
        }

        public static string OysterSpots()
        {
            const string baseS = "http://data.tfl.gov.uk/tfl/syndication/feeds/oyster-stop-locations-v1.kml?app_id={0}&app_key={1}";
            return string.Format(baseS, AppId, AppKey);
        }

        public static string Lines()
        {
            const string baseS = "https://api.tfl.gov.uk/Line?app_id={0}&app_key={1}";
            return string.Format(baseS, AppId, AppKey);
        }

        public static string SuperHighways()
        {
            const string baseS = "http://api.tfl.gov.uk/CycleSuperhighway?app_id={0}&app_key={1}";
            return string.Format(baseS, AppId, AppKey);
        }

        public static string CabWiseSpots()
        {
            const string baseS = "https://api.tfl.gov.uk/Cabwise?legacyFormat=False&forceXml=False&app_id={0}&app_key={1}";
            return string.Format(baseS, AppId, AppKey);
        }


    }
}




