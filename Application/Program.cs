using Domain.Services;
using Services.DeviceSendMerenjeServices;
using Services.ProxySaveServices;
using Services.ServerSaveDataServices;

namespace Application
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => ProxyInvalidateDataService.CheckAndUpdate());

            ISaveDataService serverSaveDataService = new ServerSaveDataService();
            var deviceSendMerenjeService = new DeviceSendMerenjeService(serverSaveDataService);
            Task.Run(()=>deviceSendMerenjeService.PosaljiNovoMerenje());





            Console.ReadLine();
        }
    }
}
