using Domain.Models;

namespace Domain.Repositories.MerenjaRepositories
{
    public interface IMerenjaRepository
    {
        public bool DodajMerenje(Merenje merenje);

        public IEnumerable<Merenje> SvaMerenja();
    }
}
