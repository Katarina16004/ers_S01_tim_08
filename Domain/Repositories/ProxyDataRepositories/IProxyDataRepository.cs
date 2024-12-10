using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories.ProxyDataRepositories
{
    public interface IProxyDataRepository
    {
        bool Dodaj(Merenje merenje);

        IEnumerable<ProxyMerenjeData> ProcitajSve();

        IEnumerable<ProxyMerenjeData> ProcitajSvaPoTipu(TipMerenja tip);

        bool Ukloni(int id);
    }
}
