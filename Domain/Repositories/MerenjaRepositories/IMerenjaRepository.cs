using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.MerenjaRepositories
{
    public interface IMerenjaRepository
    {
        public bool DodajMerenje(Merenje merenje);

        public IEnumerable<Merenje> SvaMerenja();
    }
}
