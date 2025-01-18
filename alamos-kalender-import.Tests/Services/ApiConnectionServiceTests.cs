using System.Threading.Tasks;
using DennisKae.alamos_kalender_import.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DennisKae.alamos_kalender_import.Tests.Services
{
    [Ignore("Integrationstests mit API Requests")]
    [TestClass]
    public class ApiConnectionServiceTests
    {
        private IApiConnectionService _apiConnectionService;
        
        [TestInitialize]
        public void Initialize()
        {
            ServiceProvider serviceProvider = Util.GetServiceProvider(true);
            _apiConnectionService = serviceProvider.GetRequiredService<IApiConnectionService>();
        }
        
        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetApiTokenTest()
        {
            string apiToken = await _apiConnectionService.GetApiToken();
            Assert.IsNotNull(apiToken);
        }

        [Ignore("Integrationstest")]
        [TestMethod]
        public async Task GetApiTokenWithCacheTest()
        {
            string apiToken = await _apiConnectionService.GetApiTokenWithCache();
            Assert.IsNotNull(apiToken);

            string anotherApiToken = await _apiConnectionService.GetApiTokenWithCache();
            Assert.IsNotNull(anotherApiToken);

            Assert.AreEqual(apiToken, anotherApiToken);
        }
    }
}