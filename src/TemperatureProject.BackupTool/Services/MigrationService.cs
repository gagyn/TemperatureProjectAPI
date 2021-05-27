using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TemperatureProject.BackupTool.Exception;
using TemperatureProject.Core;
using TemperatureProject.Core.Services;

namespace TemperatureProject.BackupTool.Services
{
    internal class MigrationService
    {
        private readonly IDbService dbService;
        private readonly UserInterfaceService interfaceService;

        public MigrationService(IDbService dbService, UserInterfaceService interfaceService)
        {
            this.dbService = dbService;
            this.interfaceService = interfaceService;
        }

        public void MigrateFromFileToDb()
        {
            string fileName;
            try
            {
                fileName = interfaceService.GetFileNameFromUser();
            }
            catch (FileNotFoundException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                return;
            }
            catch (ImportingCanceledException)
            {
                Console.Clear();
                Console.WriteLine("Importing was canceled.");
                return;
            }
            var records = ReadRecordsFromFile(fileName);
            InsertToDb(records);
        }

        private IEnumerable<TemperatureRecord> ReadRecordsFromFile(string fileName)
        {
            return File.ReadLines(fileName).Select(ParseLine);
        }

        private TemperatureRecord ParseLine(string line)
        {
            var splitLine = line.Split(": ");
            var date = DateTime.Parse(splitLine[0].Trim());
            var value = double.Parse(splitLine[1].Trim(), CultureInfo.InvariantCulture);
            return new TemperatureRecord
            {
                CreatedAt = date,
                Value = value,
                BasedOnRecordsCount = 1000,
                SensorNameId = "out0"
            };
        }

        private void InsertToDb(IEnumerable<TemperatureRecord> records)
        {
            dbService.Insert(records);
        }
    }
}
