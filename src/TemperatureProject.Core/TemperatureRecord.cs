using System;
using MongoDB.Bson;

namespace TemperatureProject.Core
{
    public class TemperatureRecord
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public double Value { get; set; }
        public int BasedOnRecordsCount { get; set; }
        public string SensorNameId { get; set; }

        public override string ToString() 
            => $"{this.CreatedAt}: {this.Value} (based on: {this.SensorNameId} - {this.BasedOnRecordsCount})";
    }
}