using AutoBogus;
using EStore.Api.Test.Configurations;
using EStore.API.InputModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EStore.Api.Test.Integrations.Controllers.V1
{
    public class CategoriesControllerTests : IntegrationTest 
    {
        private CategoryInputModel _categoryInputModel;
        

        public CategoriesControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
        {
        }

        [Fact]
        public async Task Register_ReturnSuccessfully_WhenCategoryIsValid()
        {
            _categoryInputModel = new AutoFaker<CategoryInputModel>(AutoBogusConfigurations.LOCATE);
            var content = new StringContent(JsonConvert.SerializeObject(_categoryInputModel), Encoding.UTF8, "application/json");
          
            var httpResponse = await _httpClient.PostAsync($"api/v1/Categories", content);

            _outputHelper.WriteLine($"{nameof(CategoriesControllerTests)}_{nameof(Register_ReturnSuccessfully_WhenCategoryIsValid)} :"+await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(System.Net.HttpStatusCode.Created, httpResponse.StatusCode);            
        }

        [Fact]
        public async Task Register_ReturnFailure_WhenCategoryIsInvalidByNameAlreadyRegistered()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_categoryInputModel), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync($"api/v1/Categories", content);

            _outputHelper.WriteLine($"{nameof(CategoriesControllerTests)}_{nameof(Register_ReturnFailure_WhenCategoryIsInvalidByNameAlreadyRegistered)} :" + await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(System.Net.HttpStatusCode.Conflict, httpResponse.StatusCode);
        }

        public async override Task InitializeAsync()
        {
            await Register_ReturnSuccessfully_WhenCategoryIsValid();
        }

    }
}
