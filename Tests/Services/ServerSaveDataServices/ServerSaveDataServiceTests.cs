using Moq;
using Domain.Services;
using Domain.Enums;
using Domain.Models;
using Services.ServerSaveDataServices;

namespace Tests.Services.ServerSaveDataServices
{
    [TestFixture]
    public class ServerSaveDataServiceTests
    {
        Mock<ISaveDataService> _serverSaveService;
        public ServerSaveDataServiceTests() => _serverSaveService = new Mock<ISaveDataService>();
        [SetUp]
        public void Setup()
        {
            _serverSaveService = new Mock<ISaveDataService>();
        }
        [Test]
        [TestCase(1, TipMerenja.DIGITALNO, "19.12.2024 13:32:35", 25, 1)]
        [TestCase(2, TipMerenja.ANALOGNO, "19.12.2024 13:32:37", 60, 2)]
        [TestCase(3, TipMerenja.ANALOGNO, "19.12.2024 13:32:34", 1013, 2)]
        public void SaveMerenjeSaIspravnimPodacima_VracaTrue(int id, TipMerenja tip, string timestamp, int value, int deviceId)
        {
            DateTime date = DateTime.Parse(timestamp);
            Merenje merenje = new(id, tip, date, value, deviceId);
            _serverSaveService.Setup(x => x.SaveMerenje(merenje)).Returns(true);

            bool uspesnoSaveMerenje = _serverSaveService.Object.SaveMerenje(merenje);
            Assert.That(uspesnoSaveMerenje, Is.True);
        }

        [Test]
        [TestCase(1, TipMerenja.DIGITALNO, "19.12.202413:32:35", 25, 1)]
        [TestCase(2, TipMerenja.ANALOGNO, "19.12.202413:32:37", 60, 2)]
        [TestCase(3, TipMerenja.ANALOGNO, "19.12.202413:32:34", 1013, 1)]
        public void SaveMerenjeSaNeispravnimPodacima_VracaFalse(int id, TipMerenja tip, string timestamp, int value, int deviceId)
        {
            bool uspeh=DateTime.TryParse(timestamp,out DateTime date);
            Merenje merenje = new(id, tip, date, value, deviceId);
            if(!uspeh)
                _serverSaveService.Setup(x => x.SaveMerenje(merenje)).Returns(false);

            bool uspesnoSaveMerenje = _serverSaveService.Object.SaveMerenje(merenje);
            Assert.That(uspesnoSaveMerenje, Is.False);
        }
    }
}
