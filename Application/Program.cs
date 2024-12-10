using Services.ProxySaveServices;

namespace Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => ProxyInvalidateDataService.CheckAndUpdate());
            Console.ReadLine();
        }
    }
}
