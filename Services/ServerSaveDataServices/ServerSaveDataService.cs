using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;

namespace Services.ServerSaveDataServices
{
    public class ServerSaveDataService : ISaveDataService
    {
        IMerenjaRepository repo = new MerenjaRepository();

        public bool SaveMerenje(Merenje merenje)
        {
            return repo.DodajMerenje(merenje);
        }
    }
}
