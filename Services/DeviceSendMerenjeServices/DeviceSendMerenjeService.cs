using Domain.Enums;
using Domain.Models;
using Domain.Repositories.DeviceRepositories;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;

namespace Services.DeviceSendMerenjeServices
{
    public class DeviceSendMerenjeService: IDeviceSendMerenjeService
    {
        static IDeviceRepository deviceRepository= new DeviceRepository();
        static IMerenjaRepository merenjaRepository = new MerenjaRepository();
        ISaveDataService serverSaveDataService;
        ILoggerService loggerService;
        public bool pozvano { get; private set; } = false;
        public DeviceSendMerenjeService(ISaveDataService serverSaveDataService, ILoggerService loggerService)
        {
            this.serverSaveDataService = serverSaveDataService;
            this.loggerService = loggerService;
        }
        public async Task PosaljiNovoMerenje()
        {
            pozvano = true;
            while(true)
            {
                GenerisiMerenje();
                await Task.Delay(new Random().Next(2000,7000));
            }
        }
        public void GenerisiMerenje()
        {
            int brUredjaja = deviceRepository.SviUredjaji().Count();
            int randomUredjajIndex = new Random().Next(0, brUredjaja);
            int deviceId = deviceRepository.SviUredjaji().ToList()[randomUredjajIndex].Id;
            int idMerenja = merenjaRepository.SvaMerenja().Count() + 1;
            TipMerenja tip = (TipMerenja)new Random().Next(0, Enum.GetValues(typeof(TipMerenja)).Length);
            Merenje merenje = new Merenje(idMerenja, tip, DateTime.Now, new Random().Next(50, 500), deviceId);

            loggerService.Log("Izmereno novo merenje...");

            serverSaveDataService.SaveMerenje(merenje);
        }
    }
}
