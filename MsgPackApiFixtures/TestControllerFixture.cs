using MessagePack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MsgPackApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using MsgPackLib;

namespace MsgPackSimpleFixtures
{
    public class TestController
    {
        [Fact]
        public async Task CanGet()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/values");

                response.EnsureSuccessStatusCode();

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task PostMsgPack()
        {
            var ct = ComplexType1.Sample();
            var bytes = MessagePackSerializer.Serialize(ct);
            ByteArrayContent bae = new ByteArrayContent(bytes);
            
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/values/sendmessage", bae);
                var result = await response.Content.ReadAsAsync<ComplexType1>();
                Assert.Equal(ct.Simple1.First().Id, result.Simple1.First().Id);
                
            }

        }

    }


    public class TestClientProvider : IDisposable
    {
        private TestServer server;

        public HttpClient Client { get; private set; }

        public TestClientProvider()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());

            Client = server.CreateClient();
        }

        public void Dispose()
        {
            server?.Dispose();
            Client?.Dispose();
        }
    }
}
