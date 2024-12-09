using Domain.Enums;
using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServerReadDataServices
{
    public class ServerReadDataService : IReadData
    {
        IMerenjaRepository repo = new MerenjaRepository();

        public IEnumerable<Merenje> ProcitajMerenjaPoTipu(TipMerenja tip)
        {
            List<Merenje> pronadjena = [];

            foreach(Merenje m in repo.SvaMerenja())
            {
                if(m.Tip == tip)
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
            throw new NotImplementedException();
        }

        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId)
        {
            throw new NotImplementedException();
        }
    }
}
