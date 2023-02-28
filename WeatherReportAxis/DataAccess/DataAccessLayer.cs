using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WeatherReportAxis.Entities;

namespace WeatherReportAxis.DataAccess
{
    internal class DataAccessLayer
    {
        static HttpClient client = new HttpClient();

        private readonly string baseURL = "https://opendata-download-metobs.smhi.se";
        public DataAccessLayer() 
        {
            SetHeaders(); 
        }

        public List<TemperatureData.Station> GetAllWeatherStations()
        {
            return GetTemperatureData();
        } 

        public List<RainfallData.Value> GetRainfallInLund()
        {
            return GetRainfallData();
        }

        private List<RainfallData.Value> GetRainfallData()
        {
            // Create empty list to contain the data
            List<RainfallData.Value> rainfallData = new List<RainfallData.Value>();
            
            try
            {
                // Deserialize api response data
                var objects = JsonConvert.DeserializeObject<RainfallData>(GetResponseBody("/api/version/latest/parameter/23/station/53430/period/latest-months/data.json").GetAwaiter().GetResult());

                // Add every value to the list
                foreach (var rainfall in objects.value)
                {
                    rainfallData.Add(rainfall);
                }
            } catch(Exception ex)
            {

            }

            return rainfallData;  
        }
        private List<TemperatureData.Station> GetTemperatureData()
        {
            // Create empty list to contain the data
            List<TemperatureData.Station> weatherStations = new List<TemperatureData.Station>();

            try
            {
                var responseBody = GetResponseBody("/api/version/1.0/parameter/1/station-set/all/period/latest-hour/data.json").GetAwaiter().GetResult();
                
                if(responseBody == null)
                {
                    return weatherStations;
                }
                
                // Deserialize api response data
                var objects = JsonConvert.DeserializeObject<TemperatureData>(responseBody);

                // Add every value to the list
                foreach (var station in objects.station)
                {
                    weatherStations.Add(station);
                }
            } catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            

            return weatherStations;
        }

        private async Task<string> GetResponseBody(string url)
        {
            try
            {
                // Get data from api using base url and parameter url
                var res = await client.GetAsync($"{baseURL}{url}");

                // Ensure that the response is successfull
                res.EnsureSuccessStatusCode();

                // Serialize response as string
                var responseBody = await res.Content.ReadAsStringAsync();
                   
                return responseBody;
            } catch (Exception ex)
            {
                return null;
            }
            
        }

        private void SetHeaders()
        {
            // Set request headers for api calls
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
