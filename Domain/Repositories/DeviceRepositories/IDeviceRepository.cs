using Domain.Models;

namespace Domain.Repositories.DeviceRepositories
{
    public interface IDeviceRepository
    {
        public IEnumerable<Device> SviUredjaji();
    }
}
