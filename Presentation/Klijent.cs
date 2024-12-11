using Domain.Repositories.DeviceRepositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine("\n\tIzaberite opciju:");
                Console.WriteLine("1. Citanje svih merenja uredjaja");
                Console.WriteLine("2. Citanje poslednjeg merenja uredjaja");
                Console.WriteLine("3. Citanje poslednjeg merenja svakog uredjaja");
                Console.WriteLine("4. Citanje svih digitalnih merenja");
                Console.WriteLine("5. Citanje svih analognih merenja");
                Console.WriteLine("6. Izadji");
                Console.Write("Unesite broj opcije: ");

                string unos = Console.ReadLine();

                if (unos == "1")
                {
                    Console.WriteLine("Lista uredjaja: ");
                    var uredjaji=deviceRepository.SviUredjaji();
                    foreach(var uredjaj in uredjaji)
                    {
                        Console.WriteLine($"Naziv: {uredjaj.Naziv} , ID: {uredjaj.Id}");
                    }
                        
                    Console.Write("Unesite ID uredjaja: ");
                    if (int.TryParse(Console.ReadLine(), out int deviceId))
                    {
                        var merenja_uredjaja = proxyReadService.ProcitajSvaMerenjaPoDeviceId(deviceId);
                        Console.WriteLine($"Merenja uredjaja {deviceId}:");
                        foreach (var merenje in merenja_uredjaja)
                        {
                            Console.WriteLine(merenje);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Morate uneti ispravan ID");
                    }
                }
              
            }
        }
    }
}
