using System;
using System.IO;
using Newtonsoft.Json;
using TemperatureProject.Core.Services;

namespace TemperatureProject.BackupTool.Services
{
    internal class DbBackupService
    {
        private readonly IDbService dbService;

        public DbBackupService(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public void MakeBackup()
        {
            var temperatureRecords = dbService.GetAll();
            var json = JsonConvert.SerializeObject(temperatureRecords, Formatting.Indented);
            File.WriteAllText($"temperature_records_backup_{DateTime.Now.ToShortDateString()}.json", json);
        }
    }
}
