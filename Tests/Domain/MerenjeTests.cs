using Domain.Enums;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Domain
{
    [TestFixture]
    public class MerenjeTests
    {
        [Test]
        [TestCase(1, TipMerenja.DIGITALNO, "19.12.2024 13:32:35", 25, 1)]
        [TestCase(2, TipMerenja.ANALOGNO, "19.12.2024 13:32:37", 60, 2)]
        [TestCase(3, TipMerenja.ANALOGNO, "19.12.2024 13:32:34", 1013, 3)]
        public void MerenjeKonstruktor_Ok(int id, TipMerenja tip, string timestamp, int value, int deviceId)
        {
            DateTime date = DateTime.Parse(timestamp);
            Merenje merenje = new(id, tip, date, value, deviceId);

            Assert.That(merenje, Is.Not.Null);
            Assert.That(merenje.Id, Is.EqualTo(id));
            Assert.That(merenje.Tip, Is.EqualTo(tip));
            Assert.That(merenje.Timestamp, Is.EqualTo(date));
            Assert.That(merenje.Value, Is.EqualTo(value));
            Assert.That(merenje.DeviceId, Is.EqualTo(deviceId));
        }

        [Test]
        public void MerenjeKonstruktor_ProveraProperty()
        {
            Merenje merenje = new(1, TipMerenja.DIGITALNO, DateTime.Now, 25, 1);

            merenje.Value = 30;
            merenje.DeviceId = 2;

            Assert.That(merenje,Is.Not.Null);
            Assert.That(merenje.Value, Is.EqualTo(30));
            Assert.That(merenje.DeviceId, Is.EqualTo(2));
        }
    }
}
