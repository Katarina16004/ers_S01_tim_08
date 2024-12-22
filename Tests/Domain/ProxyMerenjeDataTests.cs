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
    public class ProxyMerenjeDataTests
    {
        [Test]
        [TestCase("22.12.2024 13:32:35")]
        [TestCase("22.12.2024 13:32:36")]
        [TestCase("22.12.2024 13:32:37")]
        public void ProxyMerenjeDataKonstruktor_Ok(string lastAccessedForRead)
        {
            DateTime lastAccessed = DateTime.Parse(lastAccessedForRead);
            ProxyMerenjeData proxyMerenje = new (lastAccessed);

            Assert.That(proxyMerenje, Is.Not.Null);
            Assert.That(proxyMerenje.LastAccessedForRead, Is.EqualTo(lastAccessed));
        }
        [Test]
        public void ProxyMerenjeDataKonstruktor_ProveraProperty()
        {
            DateTime staroLastAccessed = DateTime.Now;
            ProxyMerenjeData proxyMerenje = new(staroLastAccessed);

            DateTime novoLastAccessed = DateTime.Parse("22.12.2024 13:32:37");
            proxyMerenje.LastAccessedForRead = novoLastAccessed;

            Assert.That(proxyMerenje.LastAccessedForRead, Is.EqualTo(novoLastAccessed));
        }
    }
}
