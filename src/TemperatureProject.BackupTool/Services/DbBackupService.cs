using System;
using System.IO;
using Newtonsoft.Json;

namespace TemperatureProject.BackupTool.Services
{
    internal class DbBackupService
    {
        private readonly DbService dbService;

        public DbBackupService(DbService dbService)
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
