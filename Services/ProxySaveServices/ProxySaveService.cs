using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ProxySaveServices
{
    public class ProxySaveService : IProxySave
    {
        private readonly IReadData serverReadDataService;
        private readonly IMerenjaRepository merenjaRepo;
        private Dictionary<int, (List<Merenje> podaci, DateTime poslednjiPristup)> lokalnaMerenja;

        public ProxySaveService(IReadData serverReadDataService,IMerenjaRepository merenjaRepo)
        {
            this.serverReadDataService = serverReadDataService;
            this.merenjaRepo = merenjaRepo;
            lokalnaMerenja = new Dictionary<int, (List<Merenje> podaci, DateTime poslednjiPristup)>();
        }
        public Dictionary<int, (List<Merenje> podaci, DateTime poslednjiPristup)> GetLokalnaMerenja()
        {
            return lokalnaMerenja;
        }
        public bool AzurirajLokalnePodatke(int deviceId)
        {
            if (!lokalnaMerenja.ContainsKey(deviceId))
            {
                lokalnaMerenja[deviceId] = (new List<Merenje>(), DateTime.Now);
            }

            var svaMerenjaPoId=serverReadDataService.ProcitajSvaMerenjaPoDeviceId(deviceId);
            var lokalniPodaci = lokalnaMerenja[deviceId].podaci;
            foreach (var ms in svaMerenjaPoId)
            {
                bool postoji = false;
                foreach (var lm in lokalniPodaci)
                {
                    if (lm.Id == ms.Id)
                    {
                        postoji = true;
                        break;
                    }
                }

                if (!postoji)
                {
                    lokalniPodaci.Add(ms);
                }
            }
            lokalnaMerenja[deviceId] = (lokalniPodaci, DateTime.Now);
            return true;
        }
        public bool OčistiZastarelePodatke()
        {
            throw new NotImplementedException();
        }
    }
}
