using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServerSaveDataServices
{
    public class ServerSaveDataService : ISaveData
    {
        IMerenjaRepository repo = new MerenjaRepository();

        public bool SaveMerenje(Merenje merenje)
        {
            return repo.DodajMerenje(merenje);
        }
    }
}
