using Domain.Enums;
using Domain.Models;
using Domain.Repositories.DeviceRepositories;
using Domain.Repositories.ProxyDataRepositories;
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
        private readonly ILoggerService fileLoggerService;

        public Klijent(IProxyReadService proxyReadService,IDeviceRepository deviceRepository, ILoggerService fileLoggerService)
        {
            this.proxyReadService = proxyReadService;
            this.deviceRepository = deviceRepository;
            this.fileLoggerService = fileLoggerService;
        }

        public void Meni()
        {
            fileLoggerService.Log("POKRENUTA APLIKACIJA");
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
                    fileLoggerService.Log($"Korisnik je izabrao opciju {unos}");
                    Console.WriteLine("\n\t------------------------\n\tLista uredjaja\n\t------------------------ ");

                    foreach (var uredjaj in uredjaji)
                    {
                        Console.WriteLine($"\tNaziv: {uredjaj.Naziv} , ID: {uredjaj.Id}");
                    }

                    Console.Write("\t------------------------\n\nUnesite ID uredjaja: ");
                    if (int.TryParse(Console.ReadLine(), out int deviceId))
                    {
                        if (deviceId < 0 || deviceId > 2)
                        {
                            fileLoggerService.Log("Korisnik je uneo nepostojeci ID-a uredjaja");
                            Console.WriteLine("\nMorate uneti ID dostupnih uredjaja!\n");
                        }                           
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
                        fileLoggerService.Log("Korisnik je uneo neispravan format ID-a uredjaja");
                        Console.WriteLine("\nMorate uneti broj uredjaja!\n");
                    }
                }
                else if (unos == "2")
                {
                    fileLoggerService.Log($"Korisnik je izabrao opciju {unos}");
                    Console.WriteLine("\n\t------------------------\n\tLista uredjaja\n\t------------------------ ");

                    foreach (var uredjaj in uredjaji)
                    {
                        Console.WriteLine($"\tNaziv: {uredjaj.Naziv} , ID: {uredjaj.Id}");
                    }

                    Console.Write("\t------------------------\n\nUnesite ID uredjaja: ");
                    if (int.TryParse(Console.ReadLine(), out int deviceId))
                    {
                        if (deviceId < 0 || deviceId > 2)
                        {
                            fileLoggerService.Log("Korisnik je uneo nepostojeci ID-a uredjaja");
                            Console.WriteLine("\nMorate uneti ID dostupnih uredjaja!\n");
                        }
                        else
                        {
                            
                            var najnovije_merenje_uredjaja = proxyReadService.ProcitajNajnovijeMerenjePoDeviceId(deviceId);
                            Console.WriteLine($"\nNajnovije merenje uredjaja {deviceId}\n------------------------");
                            Console.WriteLine(najnovije_merenje_uredjaja);
                        }
                    }
                    else
                    {
                        fileLoggerService.Log("Korisnik je uneo neispravan format ID-a uredjaja");
                        Console.WriteLine("\nMorate uneti broj uredjaja!\n");
                    }
                }
                else if (unos == "3")
                {
                    fileLoggerService.Log($"Korisnik je izabrao opciju {unos}");
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
                    fileLoggerService.Log($"Korisnik je izabrao opciju {unos}");
                    Console.WriteLine("\nSva DIGITALNA merenja\n------------------------");
                    var digitalna_merenja = proxyReadService.ProcitajMerenjaPoTipu(TipMerenja.DIGITALNO);
                    foreach (var merenje in digitalna_merenja)
                    {
                        Console.WriteLine($"Uredjaj {merenje.DeviceId}: {merenje}");
                    }
                }
                else if (unos == "5")
                {
                    fileLoggerService.Log($"Korisnik je izabrao opciju {unos}");
                    Console.WriteLine("\nSva ANALOGNA merenja\n------------------------");
                    var analogna_merenja = proxyReadService.ProcitajMerenjaPoTipu(TipMerenja.ANALOGNO);
                    foreach (var merenje in analogna_merenja)
                    {
                        Console.WriteLine($"Uredjaj {merenje.DeviceId}: {merenje}");
                    }
                }
                else if (unos == "6")
                {
                    fileLoggerService.Log($"Korisnik je izabrao opciju {unos}");
                    fileLoggerService.Log("APLIKACIJA ISKLJUCENA");
                    Environment.Exit(0);
                }
                else
                {
                    fileLoggerService.Log("Korisnik je izabrao opciju koja ne postoji");
                    Console.WriteLine("\nMorate uneti opcije sa liste!");
                }
            }
        }
        public void PokreniTaskove()
        {
            Task.Run(() => ProxyInvalidateDataService.CheckAndUpdate());

            ISaveDataService serverSaveDataService = new ServerSaveDataService();
            Task.Run(() => new DeviceSendMerenjeService(serverSaveDataService,fileLoggerService).PosaljiNovoMerenje());
        }
    }
}
