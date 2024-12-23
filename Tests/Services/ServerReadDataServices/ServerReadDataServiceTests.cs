using Domain.Enums;
using Domain.Models;
using Domain.Repositories.DeviceRepositories;
using Domain.Repositories.MerenjaRepositories;
using Moq;
using Services.ServerReadDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services.ServerReadDataServices
{
    [TestFixture]
    public class ServerReadDataServiceTests
    {
        Mock<IMerenjaRepository> _merenjaRepo;
        Mock<IDeviceRepository> _deviceRepo;
        ServerReadDataService _serverReadService;
        public ServerReadDataServiceTests()
        {
            _merenjaRepo=new Mock<IMerenjaRepository>();
            _deviceRepo=new Mock<IDeviceRepository>();
            _serverReadService = new ServerReadDataService(_merenjaRepo.Object, _deviceRepo.Object);
        }
        [SetUp]
        public void Setup()
        {
            _merenjaRepo = new Mock<IMerenjaRepository>();
            _deviceRepo = new Mock<IDeviceRepository>();
            _serverReadService = new ServerReadDataService(_merenjaRepo.Object, _deviceRepo.Object);
        }

        [Test]
        [TestCase(TipMerenja.DIGITALNO)]
        [TestCase(TipMerenja.ANALOGNO)]
        public void ProcitajMerenjaPoTipu_VracaMerenja(TipMerenja tip)
        {
            var merenja = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Now, 100, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Now, 200, 2),
                new Merenje(3, TipMerenja.DIGITALNO, DateTime.Now, 300, 1)
            };

            _merenjaRepo.Setup(repo => repo.SvaMerenja()).Returns(merenja);
            var vraceno = _serverReadService.ProcitajMerenjaPoTipu(tip);

            Assert.IsNotNull(vraceno);
            foreach (var merenje in vraceno)
            {
                Assert.AreEqual(tip, merenje.Tip);
            }
        }
        [Test]
        [TestCase(TipMerenja.DIGITALNO)]
        [TestCase(TipMerenja.ANALOGNO)]
        public void ProcitajMerenjaPoTipu_VracaPraznuListu(TipMerenja tip)
        {
            _merenjaRepo.Setup(repo => repo.SvaMerenja()).Returns(new List<Merenje>());

            var vraceno = _serverReadService.ProcitajMerenjaPoTipu(tip);

            Assert.IsNotNull(vraceno);
            Assert.AreEqual(0, vraceno.Count());
        }

        [Test]
        public void ProcitajNajnovijeMerenjePoDeviceId_VracaNajnovijeMerenje()
        {
            var merenja = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 25, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 60, 2),
                new Merenje(3, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:40"), 1013, 1)//najnovije 
            };

            _merenjaRepo.Setup(repo => repo.SvaMerenja()).Returns(merenja);

            var najnovijeMerenje = _serverReadService.ProcitajNajnovijeMerenjePoDeviceId(1);

            Assert.IsNotNull(najnovijeMerenje);
            Assert.AreEqual(3, najnovijeMerenje.Id);
        }
        [Test]
        public void ProcitajNajnovijeMerenjeZaSvakiDevice_VracaMerenja()
        {
            var merenja = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 25, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 60, 2),
                new Merenje(3, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:40"), 1013, 1)
            };
            var uredjaji = new List<Device>
            {
                new Device(1, "Uredjaj1"),
                new Device(2, "Uredjaj2")
            };

            _merenjaRepo.Setup(repo => repo.SvaMerenja()).Returns(merenja);
            _deviceRepo.Setup(repo => repo.SviUredjaji()).Returns(uredjaji);

            var najnovijaMerenja = _serverReadService.ProcitajNajnovijeMerenjeZaSvakiDevice();

            Assert.IsNotNull(najnovijaMerenja);
            Assert.AreEqual(2, najnovijaMerenja.Count());
            Assert.AreEqual(3, najnovijaMerenja.First(m => m.DeviceId == 1).Id);
            Assert.AreEqual(2, najnovijaMerenja.First(m => m.DeviceId == 2).Id);
        }
        [Test]
        public void ProcitajSvaMerenjaPoDeviceId_VracaSvaMerenjaZaDevice()
        {
            var merenja = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 25, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 60, 1),
                new Merenje(3, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:40"), 1013, 1)
            };

            _merenjaRepo.Setup(repo => repo.SvaMerenja()).Returns(merenja);

            var svaMerenja = _serverReadService.ProcitajSvaMerenjaPoDeviceId(1);
            Assert.IsNotNull(svaMerenja);
            Assert.AreEqual(3, svaMerenja.Count());
            Assert.IsTrue(svaMerenja.All(m => m.DeviceId == 1));
        }
        [Test]
        public void ProcitajSvaMerenjaPoDeviceId_NemaMerenjaZaDevice()
        {
            var merenja = new List<Merenje>
            {
                new Merenje(1, TipMerenja.DIGITALNO, DateTime.Parse("19.12.2024 13:32:35"), 25, 1),
                new Merenje(2, TipMerenja.ANALOGNO, DateTime.Parse("19.12.2024 13:32:37"), 60, 2)
            };

            _merenjaRepo.Setup(repo => repo.SvaMerenja()).Returns(merenja);
            var svaMerenja = _serverReadService.ProcitajSvaMerenjaPoDeviceId(3);

            Assert.IsNotNull(svaMerenja);
            Assert.AreEqual(0, svaMerenja.Count());
        }
    }
}
