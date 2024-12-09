using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.DeviceRepositories
{
    public interface IDeviceRepository
    {
        public IEnumerable<Device> SviUredjaji();
    }
}
