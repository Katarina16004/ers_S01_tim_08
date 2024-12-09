using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.DeviceRepositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private static List<Device> uredjaji = new List<Device>();

        static DeviceRepository()
        {
            uredjaji = new List<Device>
            {
                new Device(1,"TermoPro"),
                new Device(2,"VoltGuard")
            };
        }
        public IEnumerable<Device> SviUredjaji()
        {
            return uredjaji;
        }
    }
}
