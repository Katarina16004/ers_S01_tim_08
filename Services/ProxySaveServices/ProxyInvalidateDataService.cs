using Domain.Models;
using Domain.Repositories.ProxyDataRepositories;
using Domain.Services;
using Services.FileLoggerServices;

namespace Services.ProxySaveServices
{
    // ovo je proxy update
    public class ProxyInvalidateDataService : IProxyInvalidateDataService
    {
        static IProxyDataRepository proxyDataRepository = new ProxyDataRepository();
        static readonly ILoggerService loggerService = new FileLoggerService("log.txt");
        public static async Task CheckAndUpdate()
        {
            while (true)
            {
                await Task.Delay(10000); // na svakih 5 minuta proveri jel ima promena //10sek zbog provere
                loggerService.Log("Provera zastarelih podataka...");
                List<ProxyMerenjeData> proxy_data = proxyDataRepository.ProcitajSve().ToList();
                int id_merenja_za_brisanje = -1;

                foreach (var data in proxy_data)
                {
                    {
                        if ((DateTime.Now - data.LastAccessedForRead).Hours >= 24)
                        {
                            id_merenja_za_brisanje = data.Id;
                        }
                    }
                    if (id_merenja_za_brisanje != -1)
                        proxyDataRepository.Ukloni(id_merenja_za_brisanje);

                }

               
            }
        }
    }
}
