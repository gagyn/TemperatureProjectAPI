using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TemperatureProject.BackupTool.Exception;

namespace TemperatureProject.BackupTool.Services
{
    internal class UserInterfaceService
    {
        public string GetFileNameFromUser()
        {
            var filesNames = GetFilesNames();
            var fileName = GetNameFromKeyboard(filesNames);

            Console.Clear();
            Console.WriteLine($"Are you sure, you want to import file with name {fileName}? (y/n)");
            var keyChar = Console.ReadKey().KeyChar;
            return keyChar is 'y' or 'Y' 
                ? fileName 
                : throw new ImportingCanceledException();
        }

        private List<string> GetFilesNames()
        {
            var filesNames = Directory.GetFiles(".", "*.txt").ToList();
            if (!filesNames.Any())
            {
                throw new FileNotFoundException("Directory doesn't contain any .txt file.");
            }
            return filesNames;
        }

        private string GetNameFromKeyboard(List<string> filesNames)
        {
            var currentName = string.Empty;
            if (filesNames.Count == 1)
            {
                return filesNames.First();
            }

            while (filesNames.Count > 1)
            {
                PrintLeftFiles(filesNames, currentName);
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Enter)
                {
                    return filesNames.First();
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    throw new ImportingCanceledException();
                }
                currentName += key.KeyChar;
                filesNames.RemoveAll(x => !x.Contains(currentName));
            }

            return filesNames.Any() 
                ? filesNames.First() 
                : throw new FileNotFoundException($"File with name {currentName} not found.");
        }

        private void PrintLeftFiles(IEnumerable<string> files, string currentName)
        {
            Console.Clear();
            foreach (var filesName in files)
            {
                Console.WriteLine(filesName);
            }
            Console.Write(currentName);
        }
    }
}
