using System;
using System.Collections.Generic;
using Serilog.Core;
using Serilog.Events;
using System.IO;
using Newtonsoft.Json;
using Serilog;

namespace SportEvent.Data
{
    public class CollectionSink : ILogEventSink
    {
        private readonly List<LogEvent> logEvents;
        private readonly string jsonFilePath;

        public CollectionSink(List<LogEvent> logEvents, string jsonFilePath)
        {
            this.logEvents = logEvents;
            this.jsonFilePath = jsonFilePath;
        }

        public void Emit(LogEvent logEvent)
        {
            // Hanya menyimpan log events dengan Method POST, GET, atau PUT
   /*         if (IsAllowedMethod(logEvent))
            {
                logEvents.Add(logEvent);
                SerializeAndWriteToJsonFile();
            }*/

            logEvents.Add(logEvent);
            SerializeAndWriteToJsonFile();
        }

/*        private bool IsAllowedMethod(LogEvent logEvent)
        {
            // Mendapatkan nilai "Method" dari properti "Properties" di dalam log event
            var properties = logEvent.Properties["Properties"].ToString();
            var propertiesObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(properties);

            if (propertiesObject.TryGetValue("Method", out var methodValue) && methodValue is string method)
            {
                // Memeriksa apakah metode adalah GET, POST, atau PUT
                return method.Equals("GET", StringComparison.OrdinalIgnoreCase) ||
                       method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                       method.Equals("PUT", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }*/

        private void SerializeAndWriteToJsonFile()
        {
            var logEventsJson = JsonConvert.SerializeObject(logEvents);

            try
            {
                File.WriteAllText(jsonFilePath, logEventsJson);
            }
            catch (Exception ex)
            {
                Log.Error("Gagal menyimpan log events ke file: {Error}", ex.Message);
            }
        }
    }
}
