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
                var res = apiResponse.Content.ReadAsStringAsync().Result;
                Assert.AreEqual(HttpStatusCode.OK, apiResponse.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestWebController()
        {
            using (var server = TestServer.Create<OwinAppStartup>())
            {
                try
                {
                    HttpResponseMessage httpResponse = await server.HttpClient.GetAsync("/tag/all");
                    httpResponse.EnsureSuccessStatusCode();
                    var res = httpResponse.Content.ReadAsStringAsync().Result;
                    Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
                }
                catch (Exception exception)
                {
                    Assert.Fail(exception.Message);

                }
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
