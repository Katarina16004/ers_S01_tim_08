using Domain.Enums;
using Domain.Models;
using Domain.Repositories.DeviceRepositories;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;

namespace Services.ServerReadDataServices
{
    public class ServerReadDataService : IReadDataService
    {
        private readonly IMerenjaRepository repo;
        private readonly IDeviceRepository repoD;

        public ServerReadDataService(IMerenjaRepository merenjaRepo, IDeviceRepository deviceRepo)
        {
            repo = merenjaRepo;
            repoD = deviceRepo;
        }

        public IEnumerable<Merenje> ProcitajMerenjaPoTipu(TipMerenja tip)
        {
            List<Merenje> pronadjena = new List<Merenje>();

            foreach (Merenje m in repo.SvaMerenja())
            {
                if (m.Tip == tip)
                    pronadjena.Add(m);
            }

            return pronadjena;
        }

        public Merenje ProcitajNajnovijeMerenjePoDeviceId(int deviceId)
        {
            List<Merenje> merenjaZaDevice = new List<Merenje>();
            foreach (Merenje m in repo.SvaMerenja())
            {
                if (m.DeviceId == deviceId)
                {
                    merenjaZaDevice.Add(m);
                }
            }
            Merenje najnovije = merenjaZaDevice[0];
            foreach (Merenje m in merenjaZaDevice)
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
            List<Merenje> najnovijaMerenjaSvih = new List<Merenje>();
            foreach (Device d in repoD.SviUredjaji())
            {
                Merenje najnovijeMerenje = ProcitajNajnovijeMerenjePoDeviceId(d.Id);
                najnovijaMerenjaSvih.Add(najnovijeMerenje);
            }
            return najnovijaMerenjaSvih;
        }

        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId)
        {
            List<Merenje> svaMerenjaUredjaja = new List<Merenje>();
            foreach (Merenje m in repo.SvaMerenja())
            {
                if (m.DeviceId == deviceId)
                    svaMerenjaUredjaja.Add(m);
            }
            return svaMerenjaUredjaja;
        }
    }
}
