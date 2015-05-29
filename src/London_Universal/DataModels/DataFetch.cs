using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable InconsistentNaming

namespace London_Universal.DataModels
{
    public static class DataFetch
    {
        public static async Task<ObservableCollection<BikePointRootObject>> BikePointsTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, DataSoureUrLs.BikeAllPoints());
            var response = await new HttpClient().SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<BikePointRootObject>>(data);
        }

        public static async Task<ObservableCollection<SuperCycleRootObject>> SuperHighwaysTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, DataSoureUrLs.SuperHighways());
            var response = await new HttpClient().SendAsync(request);
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<SuperCycleRootObject>>(data);
        }


        public static async Task<string> OysterTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, DataSoureUrLs.OysterSpots());
            var response = await new HttpClient().SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> StationsTask()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, DataSoureUrLs.StationSpots());
            var response = await new HttpClient().SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }


    }

}


