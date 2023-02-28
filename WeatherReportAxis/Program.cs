using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherReportAxis.Controllers;
using WeatherReportAxis.Entities;

namespace WeatherReportAxis
{
    internal class Program
    {
        private static Controller controller = new Controller();
        static void Main(string[] args)
        {
            GetAverageTemperatureForLatestHour();

            GetRainfallInLund();

            GetTemperatureForAllWeatherStations();

            Console.ReadLine();
        }

        public static void GetTemperatureForAllWeatherStations()
        {
            // Intantiate cancellation token 
            CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            CancellationToken token = cancelTokenSource.Token;

            Task iterateWeatherStations = new Task(async () =>
            {
                List<TemperatureData.Station> temperatureList = controller.GetWeatherStations();    
               
                if(temperatureList.Count <= 0)
                {
                    Console.WriteLine("Temperature data for all weather stations could not be fetched");
                    return;
                }

                // Iterate every weather station from the api
                foreach (var weatherStation in temperatureList)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Canceled");
                        return;
                    }

                    // Intantiate result to later print to console
                    string result = weatherStation.name + ": ";

                    if (weatherStation.value != null)
                    {
                        foreach (var value in weatherStation.value)
                        {
                            // Concatinate result value to string
                            result += value.value;
                        }
                    }
                    // Log the result
                    Console.WriteLine(result);
                    result = "";

                    await Task.Delay(100);
                }
            }, token);

            Task cancelIteration = new Task(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    // If key is pressed
                    if(Console.KeyAvailable)
                    {
                        cancelTokenSource.Cancel();
                        cancelTokenSource.Dispose();
                    }
                }
                return;
                
            }, token);

            // Start tasks
            iterateWeatherStations.Start();
            cancelIteration.Start();

            Console.WriteLine();    
        }

        public static void GetRainfallInLund()
        {
            // Intantiate variables
            float totalRainfall = 0;
            List<string> dates = new List<string>();

            List<RainfallData.Value> rainfallData = controller.GetRainfallInLund();
            
            if(rainfallData.Count <= 0)
            {
                Console.WriteLine("Data for rainfall could be fetched");
                return;
            }

            // Iterate all rainfall objects returned from api
            foreach (var date in rainfallData)
            {
                // Parse the result and add to string (second parameter enables parsing float numbers seperated by dot)
                totalRainfall += float.Parse(date.value, CultureInfo.InvariantCulture.NumberFormat);
                // Add the object date to list
                dates.Add(date._ref);
            }
            // Log the result
            if(dates.Count > 0)
            {
                Console.WriteLine($"Between {dates.First()} and {dates.Last()} the total Rainfall in Lund was {totalRainfall} millimeters");
                Console.WriteLine();
            } else
            {
                Console.WriteLine("Data for rainfall could be fetched");
            }
            
        }

        public static void GetAverageTemperatureForLatestHour()
        {
            // Instantiate variables
            float sumTemperature = 0;
            int nbrOfMeasurements = 0;
            float averageTemperature = 0;

            // Iterate every weather station returned from api
            foreach (var weatherStation in controller.GetWeatherStations())
            {
                // If the station has the value property
                if (weatherStation.value != null)
                {
                    foreach (var tempValue in weatherStation.value)
                    { 
                        // If the value property in the current object exists
                        if (tempValue.value != null)
                        {
                            // Parse the result and add to string (second parameter enables parsing float numbers seperated by dot)
                            sumTemperature += float.Parse(tempValue.value, CultureInfo.InvariantCulture.NumberFormat);
                            // Increase number of measurments by 1
                            nbrOfMeasurements += 1;
                        }
                    }
                }
            }

            if(nbrOfMeasurements > 0)
            {
                // Calculate average temperature
                averageTemperature = sumTemperature / nbrOfMeasurements;

                // Log result

                Console.WriteLine($"The average temperature in Sweden for the last hours was {averageTemperature} degrees");
                Console.WriteLine();

            } else
            {
                Console.WriteLine("Data for temperature could not be fetched");
            }

        }
    }
}
