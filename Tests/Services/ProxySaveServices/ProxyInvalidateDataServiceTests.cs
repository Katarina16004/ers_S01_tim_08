using Domain.Models;
using Domain.Repositories.ProxyDataRepositories;
using Domain.Services;
using Moq;
using Services.ProxySaveServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services.ProxySaveServices
{
    [TestFixture]
    public class ProxyInvalidateDataServiceTests
    {
        Mock<IProxyDataRepository> _proxyDataRepo;
        Mock<ILoggerService> _loggerService;

        public ProxyInvalidateDataServiceTests()
        {
            _proxyDataRepo = new Mock<IProxyDataRepository>();
            _loggerService = new Mock<ILoggerService>();
        }

        [SetUp]
        public void Setup()
        {
            _proxyDataRepo = new Mock<IProxyDataRepository>();
            _loggerService = new Mock<ILoggerService>();
        }
        [Test]
        public async Task CheckAndUpdate_PozivMetode()
        {
            var task = ProxyInvalidateDataService.CheckAndUpdate();
            await Task.Delay(500); 

            Assert.IsTrue(ProxyInvalidateDataService.pozvano);
        }

        [Test]
        public void Proveri_NemaZastarelihPodataka()
        {
            var proxy_data = new List<ProxyMerenjeData>
            {
                new ProxyMerenjeData { Id = 1, LastAccessedForRead = DateTime.Now.AddHours(-10) },
                new ProxyMerenjeData { Id = 2, LastAccessedForRead = DateTime.Now.AddHours(-5) }
            };

            _proxyDataRepo.Setup(repo => repo.ProcitajSve()).Returns(proxy_data);

            ProxyInvalidateDataService.Proveri();

            _proxyDataRepo.Verify(repo => repo.Ukloni(It.IsAny<int>()), Times.Never());
        }

    }
}
