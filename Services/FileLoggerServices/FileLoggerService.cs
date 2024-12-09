using Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FileLoggerServices
{
    public class FileLoggerService : ILoggerService
    {
        private string _putanja;

        public FileLoggerService(string putanja="log.txt")
        {
            _putanja = putanja;
        }

        public bool Log(string poruka)
        {
            using StreamWriter sw = new(_putanja, append: true);
            sw.Write($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture)}]: {poruka}\n");
            return true;
        }
    }
}
