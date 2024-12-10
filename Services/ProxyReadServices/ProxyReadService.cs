using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;

namespace Services.ProxyReadServices
{
    public class ProxyReadService : IProxyRead
    {
        private readonly IProxyInvalidateDataService proxySave;
        private readonly IReadData serverReadDataService;
        private readonly IMerenjaRepository merenjaRepo;


        public ProxyReadService(IReadData serverReadDataService, IProxyInvalidateDataService proxySave, IMerenjaRepository merenjaRepo)
        {
            this.serverReadDataService = serverReadDataService;
            this.proxySave = proxySave;
            this.merenjaRepo = merenjaRepo;
        }

        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId)
        {
            var lokalnaMerenja = proxySave.GetLokalnaMerenja();
            var svaMerenjaPoId = serverReadDataService.ProcitajSvaMerenjaPoDeviceId(deviceId);
            if (!lokalnaMerenja.ContainsKey(deviceId))
            {
                proxySave.AzurirajLokalnePodatke(deviceId);
                return svaMerenjaPoId;
            }

            DateTime najnovijiTimestamp = lokalnaMerenja[deviceId].podaci[0].Timestamp;
            foreach (Merenje m in lokalnaMerenja[deviceId].podaci)
            {
                if (m.Timestamp > najnovijiTimestamp)
                    najnovijiTimestamp = m.Timestamp;
            }

            foreach (Merenje m in svaMerenjaPoId)
            {
                if (m.Timestamp > najnovijiTimestamp)
                {
                    proxySave.AzurirajLokalnePodatke(deviceId);
                    break;
                }
            }

            return lokalnaMerenja[deviceId].podaci;
        }
        public Merenje ProcitajNajnovijeMerenjePoDeviceId(int deviceId)
        {
            var lokalnaMerenja = proxySave.GetLokalnaMerenja();
            var najnovijeMerenjePoId = serverReadDataService.ProcitajNajnovijeMerenjePoDeviceId(deviceId);
            if (!lokalnaMerenja.ContainsKey(deviceId))
            {
                proxySave.AzurirajLokalnePodatke(deviceId);
                return najnovijeMerenjePoId;
            }

            DateTime najnovijiTimestamp = lokalnaMerenja[deviceId].podaci[0].Timestamp;
            foreach (Merenje m in lokalnaMerenja[deviceId].podaci)
            {
                if (m.Timestamp > najnovijiTimestamp)
                    najnovijiTimestamp = m.Timestamp;
            }

            if (najnovijeMerenjePoId.Timestamp > najnovijiTimestamp)
            {
                proxySave.AzurirajLokalnePodatke(deviceId);
            }

            Merenje najnovije = lokalnaMerenja[deviceId].podaci[0];
            foreach (Merenje m in lokalnaMerenja[deviceId].podaci)
            {
                if (m.Timestamp > najnovije.Timestamp)
                {
                    najnovije = m;
                }
            }
            return najnovije;
        }
        public IEnumerable<Merenje> ProcitajNajnovijeMerenjeZaSvakiDevice()
        {
            var lokalnaMerenja = proxySave.GetLokalnaMerenja();
            List<Merenje> najnovijaMerenja = new List<Merenje>();
            var najnovijaMerenjaSvih = serverReadDataService.ProcitajNajnovijeMerenjeZaSvakiDevice();
            if (!lokalnaMerenja.Any())
            {
                foreach (var m in najnovijaMerenjaSvih)
                {
                    int devId = m.DeviceId;
                    proxySave.AzurirajLokalnePodatke(devId);
                }
                return najnovijaMerenjaSvih;
            }
            foreach (var devId in lokalnaMerenja.Keys)
            {
                var najnovijeMerenjePoId = serverReadDataService.ProcitajNajnovijeMerenjePoDeviceId(devId);
                DateTime najnovijiTimestamp = lokalnaMerenja[devId].podaci.Max(m => m.Timestamp);

                if (najnovijeMerenjePoId.Timestamp > najnovijiTimestamp)
                {
                    proxySave.AzurirajLokalnePodatke(devId);
                }

                Merenje najnovije = lokalnaMerenja[devId].podaci[0];
                foreach (Merenje m in lokalnaMerenja[devId].podaci)
                {
                    if (m.Timestamp > najnovije.Timestamp)
                    {
                        najnovije = m;
                    }
                }
                najnovijaMerenja.Add(najnovije);
            }
            return najnovijaMerenja;
        }
    }
}
