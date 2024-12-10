using Domain.Enums;
using Domain.Models;

namespace Domain.Services
{
    public interface IReadDataService
    {
        public IEnumerable<Merenje> ProcitajSvaMerenjaPoDeviceId(int deviceId);

        public Merenje ProcitajNajnovijeMerenjePoDeviceId(int deviceId);

        public IEnumerable<Merenje> ProcitajNajnovijeMerenjeZaSvakiDevice();

        public IEnumerable<Merenje> ProcitajMerenjaPoTipu(TipMerenja tip);
    }
}
