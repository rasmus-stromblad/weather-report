namespace WeatherReportAxis.Entities
{
    // Objects that reflect the data returned from api
    internal class RainfallData
    {
        public Value[] value { get; set; }
        public long updated { get; set; }
        public Parameter parameter { get; set; }
        public Station station { get; set; }
        public Period period { get; set; }
        public Position[] position { get; set; }
        public Link[] link { get; set; }
        

        public class Parameter
        {
            public string key { get; set; }
            public string name { get; set; }
            public string summary { get; set; }
            public string unit { get; set; }
        }

        public class Station
        {
            public string key { get; set; }
            public string name { get; set; }
            public string owner { get; set; }
            public string ownerCategory { get; set; }
            public string measuringStations { get; set; }
            public float height { get; set; }
        }

        public class Period
        {
            public string key { get; set; }
            public long from { get; set; }
            public long to { get; set; }
            public string summary { get; set; }
            public string sampling { get; set; }
        }

        public class Value
        {
            public long from { get; set; }
            public long to { get; set; }
            public string _ref { get; set; }
            public string value { get; set; }
            public string quality { get; set; }
        }

        public class Position
        {
            public long from { get; set; }
            public long to { get; set; }
            public float height { get; set; }
            public float latitude { get; set; }
            public float longitude { get; set; }
        }

        public class Link
        {
            public string rel { get; set; }
            public string type { get; set; }
            public string href { get; set; }
        }

    }
}
