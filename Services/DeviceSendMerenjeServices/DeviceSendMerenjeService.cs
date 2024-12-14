using Domain.Enums;
using Domain.Models;
using Domain.Repositories.DeviceRepositories;
using Domain.Repositories.MerenjaRepositories;
using Domain.Repositories.ProxyDataRepositories;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DeviceSendMerenjeServices
{
    public class DeviceSendMerenjeService: IDeviceSendMerenjeService
    {
        static IDeviceRepository deviceRepository= new DeviceRepository();
        static IMerenjaRepository merenjaRepository = new MerenjaRepository();
        ISaveDataService serverSaveDataService;
        public DeviceSendMerenjeService(ISaveDataService serverSaveDataService)
        {
            this.serverSaveDataService = serverSaveDataService;

        }
        public async Task PosaljiNovoMerenje()
        {
            while(true)
            {
                int brUredjaja= deviceRepository.SviUredjaji().Count();
                int randomUredjajIndex = new Random().Next(0, brUredjaja);
                int deviceId = deviceRepository.SviUredjaji().ToList()[randomUredjajIndex].Id;
                int idMerenja= merenjaRepository.SvaMerenja().Count()+1;
                TipMerenja tip = (TipMerenja)new Random().Next(0, Enum.GetValues(typeof(TipMerenja)).Length);
                Merenje merenje = new Merenje(idMerenja, tip, DateTime.Now, new Random().Next(50,500), deviceId);

                //fileLoggerService.Log("Salje se novo merenje");
                //Console.WriteLine("Salje se novo merenje");
                serverSaveDataService.SaveMerenje(merenje);
                await Task.Delay(new Random().Next(2000,7000));
            }
        }
    }
}
