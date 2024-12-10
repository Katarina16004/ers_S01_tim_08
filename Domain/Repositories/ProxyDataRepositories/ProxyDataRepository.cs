using Domain.Models;

namespace Domain.Repositories.ProxyDataRepositories
{
    public class ProxyDataRepository : IProxyDataRepository
    {
        private static List<ProxyMerenjeData> proxy_merenja = [];

        public bool Dodaj(Merenje merenje)
        {
            foreach (var mereno in proxy_merenja)
            {
                if (mereno.Id == merenje.Id)
                    return false;
            }

            ProxyMerenjeData proxy = new()
            {
                Id = merenje.Id,
                DeviceId = merenje.DeviceId,
                Timestamp = merenje.Timestamp,
                Tip = merenje.Tip,
                Value = merenje.Value
            };

            proxy_merenja.Add(proxy);
            return true;
        }

        public IEnumerable<ProxyMerenjeData> ProcitajSve()
        {
            return proxy_merenja;
        }

        public bool Ukloni(int id)
        {
            int za_brisanje_index = -1;

            for (int i = 0; i < proxy_merenja.Count; i++)
            {
                if (proxy_merenja[i].Id == id)
                    za_brisanje_index = i;
            }

            if (za_brisanje_index == -1)
                return false;
            else
            {
                proxy_merenja.RemoveAt(za_brisanje_index);
                return true;
            }
        }
    }
}
