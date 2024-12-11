using Domain.Enums;
using Domain.Models;
using Domain.Repositories.ProxyDataRepositories;
using Domain.Services;

namespace Services.ProxyReadServices
{
    public class ProxyReadService : IProxyReadService
    {
        IReadDataService serverReadDataService;
        IProxyDataRepository proxyDataRepository = new ProxyDataRepository();

        public ProxyReadService(IReadDataService serverReadDataService)
        {
            this.serverReadDataService = serverReadDataService;
        }

        public IEnumerable<Merenje> ProcitajMerenjaPoTipu(TipMerenja tip)
        {
            var merenja_tip_lokalno = proxyDataRepository.ProcitajSvaPoTipu(tip);
            var merenja_tip_server = serverReadDataService.ProcitajMerenjaPoTipu(tip);

            if (merenja_tip_lokalno.Count() != merenja_tip_server.Count())
            {
                // do update
                foreach (var server_data in merenja_tip_server)
                {
                    if (merenja_tip_lokalno.Any(m => m.Id == server_data.Id) == false)
                    {
                        proxyDataRepository.Dodaj(server_data);
                    }
                }
            }

            var izmereno = proxyDataRepository.ProcitajSvaPoTipu(tip);
            // update last read access
            DateTime access = DateTime.Now;

            foreach (var merenje in izmereno)
            {
                merenje.LastAccessedForRead = access;
            }

            return izmereno;
        }

        public Merenje ProcitajNajnovijeMerenjePoDeviceId(int deviceId)
        {
            var merenjaZaId_lokalno = proxyDataRepository.ProcitajSve().Where(m=>m.DeviceId==deviceId);
            var najnovijeZaId_server = serverReadDataService.ProcitajNajnovijeMerenjePoDeviceId(deviceId);
            
            if (!merenjaZaId_lokalno.Any() || 
                najnovijeZaId_server.Timestamp!= merenjaZaId_lokalno.Max(m => m.Timestamp))
            {
                // do update
                var merenjaZaId_server=serverReadDataService.ProcitajSvaMerenjaPoDeviceId(deviceId);
                foreach (var server_data in merenjaZaId_server)
                {
                    if (merenjaZaId_lokalno.Any(m => m.Id == server_data.Id) == false)
                    {
                        proxyDataRepository.Dodaj(server_data);
                    }
                }
            }

            var izmereno = proxyDataRepository.ProcitajSve().Where(m => m.DeviceId == deviceId).ToList();
            // update last read access
            DateTime access = DateTime.Now;

            foreach (var merenje in izmereno)
            {
                merenje.LastAccessedForRead = access;
            }

            Merenje najnovije = izmereno[0];
            foreach(Merenje m in izmereno)
            {
                if(m.Timestamp>najnovije.Timestamp)
                    najnovije = m;
            }
            return najnovije;
        }

        public IEnumerable<Merenje> ProcitajNajnovijeMerenjeZaSvakiDevice()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId)
        {
            throw new NotImplementedException();
        }
    }
}
