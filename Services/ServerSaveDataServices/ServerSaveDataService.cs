using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;

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
