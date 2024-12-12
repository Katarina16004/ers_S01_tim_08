using Domain.Repositories.DeviceRepositories;
using Domain.Services;
using Services.DeviceSendMerenjeServices;
using Services.ProxySaveServices;
using Services.ServerSaveDataServices;

namespace Presentation
{
    public class Klijent
    {
        private readonly IProxyReadService proxyReadService;
        private readonly IDeviceRepository deviceRepository;

        public Klijent(IProxyReadService proxyReadService,IDeviceRepository deviceRepository)
        {
            this.proxyReadService = proxyReadService;
            this.deviceRepository = deviceRepository;
        }

        public void Meni()
        {
            while (true)
            {
                Console.WriteLine("\tIzaberite opciju:");
                Console.WriteLine("1. Citanje svih merenja uredjaja");
                Console.WriteLine("2. Citanje poslednjeg merenja uredjaja");
                Console.WriteLine("3. Citanje poslednjeg merenja svakog uredjaja");
                Console.WriteLine("4. Citanje svih digitalnih merenja");
                Console.WriteLine("5. Citanje svih analognih merenja");
                Console.WriteLine("6. Izadji");
                Console.Write("\nUnesite broj opcije: ");

                string unos = Console.ReadLine();

                if (unos == "1")
                {
                    Console.WriteLine("\n------------------------\nLista uredjaja\n------------------------ ");
                    var uredjaji=deviceRepository.SviUredjaji();
                    foreach(var uredjaj in uredjaji)
                    {
                        Console.WriteLine($"Naziv: {uredjaj.Naziv} , ID: {uredjaj.Id}");
                    }
                        
                    Console.Write("------------------------\nUnesite ID uredjaja: ");
                    if (int.TryParse(Console.ReadLine(), out int deviceId))
                    {
                        var merenja_uredjaja = proxyReadService.ProcitajSvaMerenjaPoDeviceId(deviceId);
                        Console.WriteLine($"\nMerenja uredjaja {deviceId}:");
                        foreach (var merenje in merenja_uredjaja)
                        {
                            Console.WriteLine(merenje);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nMorate uneti broj uredjaja!\n");
                    }
                }
              
            }
        }
        public void PokreniTaskove()
        {
            Task.Run(() => ProxyInvalidateDataService.CheckAndUpdate());

            ISaveDataService serverSaveDataService = new ServerSaveDataService();
            IDeviceSendMerenjeService deviceSendMerenjeService = new DeviceSendMerenjeService(serverSaveDataService);
            Task.Run(() => deviceSendMerenjeService.PosaljiNovoMerenje());
        }
    }
}
