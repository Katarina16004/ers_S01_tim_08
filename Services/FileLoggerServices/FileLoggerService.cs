using Domain.Services;
using System.Globalization;

namespace Services.FileLoggerServices
{
    public class FileLoggerService : ILoggerService
    {
        private string _putanja;
        private static readonly object _lock = new object();
        public FileLoggerService(string putanja = "log.txt")
        {
            _putanja = putanja;
        }

        public bool Log(string poruka)
        {
            lock (_lock)
            {
                using StreamWriter sw = new(_putanja, append: true);
                sw.Write($"[{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)}]: {poruka}\n");
            }
            return true;
        }
    }
}
