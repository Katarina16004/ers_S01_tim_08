using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Domain
{
    [TestFixture]
    public class DeviceTests
    {
        [Test]
        [TestCase(1, "Naziv1")]
        [TestCase(2, "Naziv2")]
        [TestCase(3, "Naziv3")]
        public void DeviceKonstruktor_Ok(int id, string naziv)
        {
            Device device = new Device(id,naziv);
            Assert.That(device, Is.Not.Null);
            Assert.That(device.Id, Is.EqualTo(id));
            Assert.That(device.Naziv, Is.EqualTo(naziv));   
        }
        [Test]
        public void DeviceKonstruktor_ProveraProperty()
        {
            Device device = new(1, "StariNaziv");

            device.Id = 2;
            device.Naziv = "NoviNaziv";

            Assert.That(device, Is.Not.Null);
            Assert.That(device.Id, Is.EqualTo(2));
            Assert.That(device.Naziv, Is.EqualTo("NoviNaziv"));
        }
    }
}
