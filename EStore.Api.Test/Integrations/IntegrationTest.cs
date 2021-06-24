using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EStore.Api.Test.Integrations
{
    public abstract class IntegrationTest : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        protected readonly WebApplicationFactory<Startup> _factory;
        protected readonly ITestOutputHelper _outputHelper;
        protected readonly HttpClient _httpClient;

        public IntegrationTest(WebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper)
        {
            _factory = factory;
            _outputHelper = outputHelper;
            _httpClient = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            _httpClient.Dispose();
        }

        public virtual async Task InitializeAsync()
        {
            
        }
    }
}
