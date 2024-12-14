using Domain.Models;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;

namespace Services.ServerSaveDataServices
{
    public class ServerSaveDataService : ISaveDataService
    {
        IMerenjaRepository merenjaRepository = new MerenjaRepository();

        public bool SaveMerenje(Merenje merenje)
        {
            return merenjaRepository.DodajMerenje(merenje);
        }
    }
}
