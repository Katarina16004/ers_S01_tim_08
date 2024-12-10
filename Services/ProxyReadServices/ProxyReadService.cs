using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;
using Services.ServerReadDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProxyReadServices
{
    public class ProxyReadService : IProxyRead
    {
        private readonly IProxySave proxySave;
        private readonly IReadData serverReadDataService;
        private readonly IMerenjaRepository merenjaRepo;


        public ProxyReadService(IReadData serverReadDataService, IProxySave proxySave, IMerenjaRepository merenjaRepo)
        {
            this.serverReadDataService = serverReadDataService;
            this.proxySave=proxySave;
            this.merenjaRepo = merenjaRepo;
        }
        
        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId)
        {
            var lokalnaMerenja = proxySave.GetLokalnaMerenja();
            var svaMerenjaPoId= serverReadDataService.ProcitajSvaMerenjaPoDeviceId(deviceId);
            if (lokalnaMerenja.ContainsKey(deviceId))
            {
                DateTime najnovijiTimestamp = lokalnaMerenja[deviceId].podaci[0].Timestamp;
                foreach(Merenje m in lokalnaMerenja[deviceId].podaci)
                {
                    if (m.Timestamp > najnovijiTimestamp)
                        najnovijiTimestamp = m.Timestamp;
                }

                bool azuriraj = false; 
                foreach(Merenje m in svaMerenjaPoId)
                {
                    if(m.Timestamp > najnovijiTimestamp)
                    {
                        azuriraj = true;
                        break;
                    }
                }

                if (azuriraj)
                {
                    proxySave.AzurirajLokalnePodatke(deviceId);
                }

                return lokalnaMerenja[deviceId].podaci;
            }
            else
            {
                proxySave.AzurirajLokalnePodatke(deviceId);
                return svaMerenjaPoId;
            }
        }
    }
}
