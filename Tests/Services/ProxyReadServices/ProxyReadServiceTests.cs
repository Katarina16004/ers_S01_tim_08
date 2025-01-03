using Domain.Enums;
using Domain.Models;
using Domain.Repositories.ProxyDataRepositories;
using Domain.Services;
using Moq;
using Services.ProxyReadServices;

namespace Tests.Services.ProxyReadServices
{
    [TestFixture]
    public class ProxyReadServiceTests
    {
        Mock<IReadDataService> _serverReadService;
        Mock<IProxyDataRepository> _proxyDataRepo;
        Mock<ILoggerService> _loggerService;
        ProxyReadService _proxyReadService;

        public ProxyReadServiceTests()
        {
            _serverReadService = new Mock<IReadDataService>();
            _proxyDataRepo = new Mock<IProxyDataRepository>();
            _loggerService = new Mock<ILoggerService>();

            _proxyReadService = new ProxyReadService(_serverReadService.Object, _proxyDataRepo.Object, _loggerService.Object);
        }
        [SetUp]
        public void SetUp()
        {
            _serverReadService = new Mock<IReadDataService>();
            _proxyDataRepo = new Mock<IProxyDataRepository>();
            _loggerService = new Mock<ILoggerService>();
            _proxyReadService = new ProxyReadService(_serverReadService.Object, _proxyDataRepo.Object, _loggerService.Object);
        }
        [Test]
        [TestCase(TipMerenja.DIGITALNO)]
        [TestCase(TipMerenja.ANALOGNO)]
        public void ProcitajMerenjaPoTipu_IsteDuzine(TipMerenja tip)
        {
            var merenjaLokalno = new List<ProxyMerenjeData>
            {
                new ProxyMerenjeData { Id = 1, Tip = TipMerenja.DIGITALNO,Timestamp=DateTime.Parse("19.12.2024 13:32:35"), Value = 100, DeviceId = 1, LastAccessedForRead = DateTime.Now },
                new ProxyMerenjeData { Id = 2, Tip = TipMerenja.ANALOGNO,Timestamp=DateTime.Parse("19.12.2024 13:32:37"), Value = 200, DeviceId = 2, LastAccessedForRead = DateTime.Now }
            };

            var merenjaServer = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 100, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 200, 2)
            };

            _proxyDataRepo.Setup(repo => repo.ProcitajSvaPoTipu(tip)).Returns(merenjaLokalno.Where(m => m.Tip == tip));
            _serverReadService.Setup(service => service.ProcitajMerenjaPoTipu(tip)).Returns(merenjaServer.Where(m => m.Tip == tip));

            var vraceno = _proxyReadService.ProcitajMerenjaPoTipu(tip);

            Assert.IsNotNull(vraceno);
            Assert.IsTrue(vraceno.All(m => m.Tip == tip));
            Assert.AreEqual(1, vraceno.Count());
        }
        [Test]
        public void ProcitajNajnovijeMerenjePoDeviceId_VracaMerenje()
        {
            var deviceId = 1;
            var merenjaLokalno = new List<ProxyMerenjeData>
            {
                new ProxyMerenjeData { Id = 1, Tip = TipMerenja.DIGITALNO,Timestamp=DateTime.Parse("19.12.2024 13:32:35"), Value = 100, DeviceId = 1, LastAccessedForRead = DateTime.Now },
                new ProxyMerenjeData { Id = 3, Tip = TipMerenja.DIGITALNO,Timestamp=DateTime.Parse("19.12.2024 13:32:39"), Value = 400, DeviceId = 1, LastAccessedForRead = DateTime.Now },
                new ProxyMerenjeData { Id = 2, Tip = TipMerenja.ANALOGNO,Timestamp=DateTime.Parse("19.12.2024 13:32:37"), Value = 200, DeviceId = 2, LastAccessedForRead = DateTime.Now }
            };

            var merenjaServer = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 100, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 200, 2),
                new Merenje(3, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:39"), 400, 1)
            };

            _proxyDataRepo.Setup(repo => repo.ProcitajSve()).Returns(merenjaLokalno);
            _serverReadService.Setup(service => service.ProcitajNajnovijeMerenjePoDeviceId(deviceId)).Returns(merenjaServer.First());
            _serverReadService.Setup(service => service.ProcitajSvaMerenjaPoDeviceId(deviceId)).Returns(merenjaServer);

            var vraceno = _proxyReadService.ProcitajNajnovijeMerenjePoDeviceId(deviceId);
            Assert.IsNotNull(vraceno);
            Assert.AreEqual(400, vraceno.Value); 
        }
        [Test]
        public void ProcitajNajnovijeMerenjeZaSvakiDevice_NemaAžuriranjaAkoSuPodaciSinhronizovani()
        {
            var timestamp = DateTime.Now;
            var merenjaLokalno = new List<ProxyMerenjeData>
            {
                new ProxyMerenjeData { Id = 1, Tip = TipMerenja.DIGITALNO,Timestamp=DateTime.Parse("19.12.2024 13:32:35"), Value = 100, DeviceId = 1, LastAccessedForRead = DateTime.Now },
                new ProxyMerenjeData { Id = 2, Tip = TipMerenja.ANALOGNO,Timestamp=DateTime.Parse("19.12.2024 13:32:37"), Value = 200, DeviceId = 2, LastAccessedForRead = DateTime.Now }
            };
            var merenjaServer = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 100, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 200, 2)
            };
            _proxyDataRepo.Setup(repo => repo.ProcitajSve()).Returns(merenjaLokalno);
            _serverReadService.Setup(service => service.ProcitajNajnovijeMerenjeZaSvakiDevice()).Returns(merenjaServer);


            var result = _proxyReadService.ProcitajNajnovijeMerenjeZaSvakiDevice();
            Assert.AreEqual(2, result.Count());  
        }

        [Test]
        public void ProcitajSvaMerenjaPoDeviceId_VracaSvaMerenja()
        {
            var deviceId = 1;
            var merenjaLokalno = new List<ProxyMerenjeData>
            {
                new ProxyMerenjeData { Id = 1, Tip = TipMerenja.DIGITALNO,Timestamp=DateTime.Parse("19.12.2024 13:32:35"), Value = 100, DeviceId = 1, LastAccessedForRead = DateTime.Now },
                new ProxyMerenjeData { Id = 2, Tip = TipMerenja.ANALOGNO,Timestamp=DateTime.Parse("19.12.2024 13:32:37"), Value = 200, DeviceId = 2, LastAccessedForRead = DateTime.Now },
                new ProxyMerenjeData { Id = 3, Tip = TipMerenja.ANALOGNO,Timestamp=DateTime.Parse("19.12.2024 13:32:39"), Value = 180, DeviceId = 1, LastAccessedForRead = DateTime.Now }

            };

            var merenjaServer = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 100, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 200, 2),
                new Merenje(3, TipMerenja.ANALOGNO,DateTime.Parse("19.12.2024 13:32:39"),180,1)
            };

            _proxyDataRepo.Setup(repo => repo.ProcitajSve()).Returns(merenjaLokalno);
            _serverReadService.Setup(service => service.ProcitajSvaMerenjaPoDeviceId(deviceId)).Returns(merenjaServer);

            var vraceno = _proxyReadService.ProcitajSvaMerenjaPoDeviceId(deviceId);

            Assert.IsNotNull(vraceno);
            Assert.AreEqual(2, vraceno.Count()); //imamo 2 za uredjaj1
        }

    }
}
