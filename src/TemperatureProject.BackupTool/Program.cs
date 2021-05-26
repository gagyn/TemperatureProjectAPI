using System;
using System.IO;
using System.Text.Json;
using TemperatureProject.BackupTool.Services;

namespace TemperatureProject.BackupTool
{
    internal class Program
    {
        private const string ConfigFileName = "config.json";
        private static readonly DbBackupService dbBackupService;
        private static readonly MigrationService migrationService;

        static Program()
        {
            var content = File.ReadAllText(ConfigFileName);
            var config = JsonSerializer.Deserialize<Config>(content);
            var dbService = new DbService(config!.ConnectionString);
            var userInterfaceService = new UserInterfaceService();
            dbBackupService = new DbBackupService(dbService);
            migrationService = new MigrationService(dbService, userInterfaceService);
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("1. Backup database into file");
            Console.WriteLine("2. Migrate data from file to database");
            var keyChar = Console.ReadKey().KeyChar;
            Console.Clear();
            switch (keyChar)
            {
                case '1':
                    dbBackupService.MakeBackup();
                    break;
                case '2':
                    migrationService.MigrateFromFileToDb();
                    break;
            }
        }
    }
}
