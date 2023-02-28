using System.Collections.Generic;
using WeatherReportAxis.DataAccess;
using WeatherReportAxis.Entities;

namespace WeatherReportAxis.Controllers
{
    internal class Controller
    {
        DataAccessLayer dal = new DataAccessLayer();

        public List<TemperatureData.Station> GetWeatherStations()
        {
            return dal.GetAllWeatherStations();
        }

        public List<RainfallData.Value> GetRainfallInLund()
        {
            return dal.GetRainfallInLund();
        }
    }
}
