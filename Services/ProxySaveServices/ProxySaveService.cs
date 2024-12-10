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

            var svaMerenjaPoId=serverReadDataService.ProcitajSvaMerenjaPoDeviceId(deviceId);
            if(svaMerenjaPoId == null)
                return false;
            lokalnaMerenja[deviceId] = (svaMerenjaPoId.ToList(),DateTime.Now);
            return true;
        }
        public bool OčistiZastarelePodatke()
        {
            throw new NotImplementedException();
        }
    }
}
