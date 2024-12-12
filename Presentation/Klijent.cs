using Domain.Enums;
using Domain.Models;
using Domain.Repositories.DeviceRepositories;
using Domain.Services;
using Services.DeviceSendMerenjeServices;
using Services.ProxySaveServices;
using Services.ServerSaveDataServices;

namespace Presentation
{
    public class Klijent:IKlijent
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
                Console.Write("\nUnesite broj opcije: ");
                var uredjaji = deviceRepository.SviUredjaji();

                string unos = Console.ReadLine();

                if (unos == "1")
                {
                    Console.WriteLine("\n\t------------------------\n\tLista uredjaja\n\t------------------------ ");
                   
                    foreach (var uredjaj in uredjaji)
                    {
                        Console.WriteLine($"\tNaziv: {uredjaj.Naziv} , ID: {uredjaj.Id}");
                    }

                    Console.Write("\t------------------------\n\nUnesite ID uredjaja: ");
                    if (int.TryParse(Console.ReadLine(), out int deviceId))
                    {
                        if (deviceId < 0 || deviceId > 2)
                            Console.WriteLine("\nMorate uneti ID dostupnih uredjaja!\n");
                        else
                        {
                            var merenja_uredjaja = proxyReadService.ProcitajSvaMerenjaPoDeviceId(deviceId);
                            Console.WriteLine($"\nMerenja uredjaja {deviceId}\n------------------------");
                            foreach (var merenje in merenja_uredjaja)
                            {
                                Console.WriteLine(merenje);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nMorate uneti broj uredjaja!\n");
                    }
                }
                else if (unos == "2")
                {
                    Console.WriteLine("\n\t------------------------\n\tLista uredjaja\n\t------------------------ ");
                    
                    foreach (var uredjaj in uredjaji)
                    {
                        Console.WriteLine($"\tNaziv: {uredjaj.Naziv} , ID: {uredjaj.Id}");
                    }

                    Console.Write("\t------------------------\n\nUnesite ID uredjaja: ");
                    if (int.TryParse(Console.ReadLine(), out int deviceId))
                    {
                        if (deviceId < 0 || deviceId > 2)
                            Console.WriteLine("\nMorate uneti ID dostupnih uredjaja!\n");
                        else
                        {
                            var najnovije_merenje_uredjaja = proxyReadService.ProcitajNajnovijeMerenjePoDeviceId(deviceId);
                            Console.WriteLine($"\nNajnovije merenje uredjaja {deviceId}\n------------------------");
                            Console.WriteLine(najnovije_merenje_uredjaja);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nMorate uneti broj uredjaja!\n");
                    }
                }
                else if (unos == "3")
                {
                    var najnovija_merenja = proxyReadService.ProcitajNajnovijeMerenjeZaSvakiDevice();
                    Console.WriteLine("\nNajnovija merenja svih uredjaja\n------------------------");
                    foreach (var merenje in najnovija_merenja)
                    {
                        Console.Write($"Najnovije merenje uredjaja {merenje.DeviceId}:  ");
                        Console.WriteLine(merenje);
                    }
                }
                else if (unos == "4")
                {
                    Console.WriteLine("\nSva DIGITALNA merenja\n------------------------");
                    var digitalna_merenja = proxyReadService.ProcitajMerenjaPoTipu(TipMerenja.DIGITALNO);
                    foreach (var merenje in digitalna_merenja)
                    {
                        Console.WriteLine(merenje);
                    }
                }
                else if (unos == "5")
                {
                    Console.WriteLine("\nSva ANALOGNA merenja\n------------------------");
                    var analogna_merenja = proxyReadService.ProcitajMerenjaPoTipu(TipMerenja.ANALOGNO);
                    foreach (var merenje in analogna_merenja)
                    {
                        Console.WriteLine(merenje);
                    }
                }
                else if (unos == "6")
                    Environment.Exit(0);
                else
                    Console.WriteLine("\nMorate uneti opcije sa liste!");
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
