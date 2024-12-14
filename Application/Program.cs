using Domain.Repositories.DeviceRepositories;
using Domain.Services;
using Presentation;
using Services.ProxyReadServices;
using Services.ServerReadDataServices;
using Domain.Repositories.ProxyDataRepositories;
using Domain.Repositories.MerenjaRepositories;
using Services.FileLoggerServices;

namespace Application
{
    public class Program
    {
        static void Main(string[] args)
        {

            ILoggerService fileLoggerService = new FileLoggerService();
            IMerenjaRepository merenjaRepo = new MerenjaRepository();
            IDeviceRepository devRepo= new DeviceRepository();
            IProxyDataRepository proxyDataRepository = new ProxyDataRepository();
            IReadDataService serverReadDataService = new ServerReadDataService(merenjaRepo, devRepo);
            IProxyReadService proxyReadService = new ProxyReadService(serverReadDataService, proxyDataRepository,fileLoggerService);
            

           
            Klijent klijent = new Klijent(proxyReadService, devRepo,fileLoggerService);
            klijent.PokreniTaskove();
            klijent.Meni();
            

            Console.ReadLine();
        }
    }
}
