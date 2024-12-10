using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IProxyRead
    {
        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId);
        public Merenje ProcitajNajnovijeMerenjePoDeviceId(int deviceId);
        public IEnumerable<Merenje> ProcitajNajnovijeMerenjeZaSvakiDevice();
    }
}
