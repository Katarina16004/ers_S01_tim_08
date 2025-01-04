using Domain.Enums;
using Domain.Models;
using Domain.Repositories.DeviceRepositories;
using Domain.Repositories.MerenjaRepositories;
using Domain.Services;
using Moq;
using Services.DeviceSendMerenjeServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services.DeviceSendMerenjeServices
{
    [TestFixture]
    public class DeviceSendMerenjeServiceTests
    {
        Mock<IDeviceRepository> _deviceRepo;
        Mock<IMerenjaRepository> _merenjaRepo;
        Mock<ILoggerService> _loggerService;
        Mock<ISaveDataService> _saveDataService;
        DeviceSendMerenjeService _sendMerenjeService;

        public DeviceSendMerenjeServiceTests()
        {
            _deviceRepo = new Mock<IDeviceRepository>();
            _merenjaRepo = new Mock<IMerenjaRepository>();
            _loggerService = new Mock<ILoggerService>();
            _saveDataService = new Mock<ISaveDataService>();

            _sendMerenjeService = new DeviceSendMerenjeService(_saveDataService.Object, _loggerService.Object);
        }

        [SetUp]
        public void Setup()
        {
            _deviceRepo = new Mock<IDeviceRepository>();
            _merenjaRepo = new Mock<IMerenjaRepository>();
            _loggerService = new Mock<ILoggerService>();
            _saveDataService = new Mock<ISaveDataService>();

            _sendMerenjeService = new DeviceSendMerenjeService(_saveDataService.Object, _loggerService.Object);
        }
        [Test]
        public void GenerisiMerenje_GeneriseILoguje()
        {
            var uredjaji = new List<Device>
            {
                new Device (1,"Uredjaj1")
            };

            _deviceRepo.Setup(repo => repo.SviUredjaji()).Returns(uredjaji);
            _sendMerenjeService.GenerisiMerenje();

            _loggerService.Verify(logger => logger.Log(It.IsAny<string>()), Times.AtLeastOnce());
            _saveDataService.Verify(save => save.SaveMerenje(It.IsAny<Merenje>()), Times.AtLeastOnce());
        }
        [Test]
        public void GenerisiMerenje_IspravnostVrednostiMerenja()
        {
            var uredjaji = new List<Device>
            { 
                new Device (1,"Uredjaj1")
            };

            _deviceRepo.Setup(repo => repo.SviUredjaji()).Returns(uredjaji);
            _sendMerenjeService.GenerisiMerenje();

            _saveDataService.Verify(save => save.SaveMerenje(It.Is<Merenje>(m =>m.DeviceId == 1 || m.DeviceId == 2 
                && m.Value >= 50 && m.Value <= 500 && Enum.IsDefined(typeof(TipMerenja), m.Tip))), Times.AtLeastOnce());
        }
        [Test]
        public async Task PosaljiNovoMerenje_PozivMetode()
        {
            var task = _sendMerenjeService.PosaljiNovoMerenje();
            await Task.Delay(500);

            Assert.IsTrue(_sendMerenjeService.pozvano);
        }
    }
}
