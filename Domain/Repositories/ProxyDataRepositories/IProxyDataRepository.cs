using Domain.Models;

namespace Domain.Repositories.ProxyDataRepositories
{
    public interface IProxyDataRepository
    {
        bool Dodaj(Merenje merenje);

        IEnumerable<ProxyMerenjeData> ProcitajSve();

        bool Ukloni(int id);
    }
}
