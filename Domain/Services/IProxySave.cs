using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IProxySave
    {
        public bool AzurirajLokalnePodatke(int deviceId);
        public Dictionary<int, (List<Merenje> podaci, DateTime poslednjiPristup)> GetLokalnaMerenja();
        public bool OcistiZastarelePodatke();
    }
}
