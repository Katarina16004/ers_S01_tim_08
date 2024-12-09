using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.MerenjaRepositories
{
    public class MerenjaRepository : IMerenjaRepository
    {
        private static List<Merenje> merenja = new List<Merenje>();

        static MerenjaRepository()
        {
            merenja = new List<Merenje>
            {
                new Merenje(1, TipMerenja.ANALOGNO, DateTime.Now, 150, 1),
                new Merenje(2, TipMerenja.DIGITALNO, DateTime.Now, 200, 2),
                new Merenje(3, TipMerenja.ANALOGNO, DateTime.Now, 300, 1)
            };
        }

        public bool DodajMerenje(Merenje merenje)
        {
            if(merenja.Count == 0)
                merenje.Id = 1;
            else
                merenje.Id = merenja.Last().Id + 1;

            merenja.Add(merenje);
            return true;
        }

        public IEnumerable<Merenje> SvaMerenja()
        {
            return merenja;
        }
    }
}
