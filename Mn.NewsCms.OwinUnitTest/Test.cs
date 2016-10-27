using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mn.NewsCms.Web;

namespace Mn.NewsCms.OwinUnitTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public async Task TestWebApi()
        {
            using (var server = TestServer.Create<OwinAppStartup>())
            {
                HttpResponseMessage apiResponse = await server.HttpClient.GetAsync("/api/Category");
                apiResponse.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestOData()
        {
            using (var server = TestServer.Create<OwinAppStartup>())
            {
                HttpResponseMessage odataResponse = await server.HttpClient.GetAsync("odata/Categories/Test.GetBestCategory");
                odataResponse.EnsureSuccessStatusCode();
                Assert.AreEqual(HttpStatusCode.OK, odataResponse.StatusCode);
            }
        }
    }
}
