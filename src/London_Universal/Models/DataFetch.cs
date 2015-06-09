using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using London_Universal.DataModels;
using Newtonsoft.Json;

// ReSharper disable UnusedVariable

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable InconsistentNaming

namespace London_Universal.Models
{
    public static class DataFetch
    {
        public static async Task GetDataTask()
        {
            var Bikerequest = new HttpRequestMessage(HttpMethod.Get, Constants.BikeAllPoints());
            var Bikeresponse = await new HttpClient().SendAsync(Bikerequest);
            var Bikedata = await Bikeresponse.Content.ReadAsStringAsync();
            var Bikereturndata = JsonConvert.DeserializeObject<ObservableCollection<BikePointRootObject>>(Bikedata);

            var SuperHighwayrequest = new HttpRequestMessage(HttpMethod.Get, Constants.SuperHighways());
            var SuperHighwayresponse = await new HttpClient().SendAsync(SuperHighwayrequest);
            var SuperHighwaydata = await SuperHighwayresponse.Content.ReadAsStringAsync();
            var SuperHighwayreturndata =
                JsonConvert.DeserializeObject<ObservableCollection<SuperCycleRootObject>>(SuperHighwaydata);

            var CabWiserequest = new HttpRequestMessage(HttpMethod.Get, Constants.CabWiseSpots());
            var CabWiseresponse = await new HttpClient().SendAsync(CabWiserequest);
            var CabWisedata = await CabWiseresponse.Content.ReadAsStringAsync();
            var CabWisereturndata = JsonConvert.DeserializeObject<CabWiseRootObject>(CabWisedata);

            //foreach (var mapel in Bikereturndata.Select(item => new CustomMapElement(MapElementTypes.BikePoint, new Location(item.lat, item.lon), item.commonName, item.additionalProperties)))
            //{
            //    MainViewModel.TflObservableCollection.Add(mapel);
            //}

            //foreach (var mapel in SuperHighwayreturndata.Select(item => new CustomMapElement(MapElementTypes.SuperHighWay, double.Parse(item.Geography.Coordinates[1].ToString()), double.Parse(item.Geography.Coordinates[0].ToString()), item.Label)))
            //{
            //    MainViewModel.TflObservableCollection.Add(mapel);
            //}
            //
            //foreach (var mapel in CabWisereturndata.operators.operatorList.Select(item => new CustomMapElement(MapElementTypes.CabWise, item.latitude, item.longitude, item.tradingName, null, null, null, null, item.operatorId.ToString(), item.bookingsPhoneNumber, item.town, item.addressLine1, item.addressLine2, item.addressLine3 )))
            //{
            //    MainViewModel.TflObservableCollection.Add(mapel);
            //}
        }


        public static async Task<string> OysterTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Constants.OysterSpots());
            var response = await new HttpClient().SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> StationsTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Constants.Lines());
            var response = await new HttpClient().SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

    }

}


