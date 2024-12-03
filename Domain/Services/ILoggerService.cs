using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface ILoggerService // za upis u log.txt datoteku npr, moze in memory lista zavisi sta nasledi
    {
        public bool Log(string poruka);
    }
}
