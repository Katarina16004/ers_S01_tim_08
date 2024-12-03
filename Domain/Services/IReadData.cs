using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IReadData
    {
        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId);

        public Merenje ProcitajNajnovijeMerenjePoDeviceId(int deviceId);

        public IEnumerable<Merenje> ProcitajNajnovijeMerenjeZaSvakiDevice();

        public IEnumerable<Merenje> ProcitajMerenjaPoTipu(TipMerenja tip);
    }
}
