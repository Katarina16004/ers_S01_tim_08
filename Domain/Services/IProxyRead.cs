using Domain.Models;

namespace Domain.Services
{
    public interface IProxyRead
    {
        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId);
        public Merenje ProcitajNajnovijeMerenjePoDeviceId(int deviceId);
        public IEnumerable<Merenje> ProcitajNajnovijeMerenjeZaSvakiDevice();
    }
}
