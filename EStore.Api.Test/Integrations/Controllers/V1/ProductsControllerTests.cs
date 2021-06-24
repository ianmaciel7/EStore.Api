using AutoBogus;
using EStore.Api.InputModel;
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
    public class ProductsControllerTests : IntegrationTest
    {
        private CategoryInputModel _categoryInputModel;
        private SubCategoryInputModel _subCategoryInputModel;
        private ProductInputModel _productInputModel;

        public ProductsControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
        {
        }

        [Fact]
        public async Task Register_ReturnSuccessfully_WhenProductIsValid()
        {

            _categoryInputModel = new AutoFaker<CategoryInputModel>(AutoBogusConfigurations.LOCATE);
            var content = new StringContent(JsonConvert.SerializeObject(_categoryInputModel), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync($"api/v1/Categories", content);

            _subCategoryInputModel = new AutoFaker<SubCategoryInputModel>(AutoBogusConfigurations.LOCATE);
            content = new StringContent(JsonConvert.SerializeObject(_subCategoryInputModel), Encoding.UTF8, "application/json");
            httpResponse = await _httpClient.PostAsync($"api/v1/Categories/{_categoryInputModel.Name}/SubCategories", content);

            _productInputModel = new AutoFaker<ProductInputModel>(AutoBogusConfigurations.LOCATE);
            content = new StringContent(JsonConvert.SerializeObject(_productInputModel), Encoding.UTF8, "application/json");
            httpResponse = await _httpClient.PostAsync($"api/v1/Categories/{_categoryInputModel.Name}/SubCategories/{ _subCategoryInputModel.Name}/Products", content);

            _outputHelper.WriteLine($"{nameof(ProductsControllerTests)}_{nameof(Register_ReturnSuccessfully_WhenProductIsValid)} :" + await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(System.Net.HttpStatusCode.Created, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Register_ReturnFailure_WhenProductIsInvalidByNameAlreadyRegistered()
        {
            
            var content = new StringContent(JsonConvert.SerializeObject(_categoryInputModel), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync($"api/v1/Categories", content);
           
            content = new StringContent(JsonConvert.SerializeObject(_subCategoryInputModel), Encoding.UTF8, "application/json");
            httpResponse = await _httpClient.PostAsync($"api/v1/Categories/{_categoryInputModel.Name}/SubCategories", content);
        
            content = new StringContent(JsonConvert.SerializeObject(_productInputModel), Encoding.UTF8, "application/json");
            httpResponse = await _httpClient.PostAsync($"api/v1/Categories/{_categoryInputModel.Name}/SubCategories/{ _subCategoryInputModel.Name}/Products", content);

            _outputHelper.WriteLine($"{nameof(ProductsControllerTests)}_{nameof(Register_ReturnSuccessfully_WhenProductIsValid)} :" + await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(System.Net.HttpStatusCode.Conflict, httpResponse.StatusCode);
        }

        public async override Task InitializeAsync()
        {
            await Register_ReturnSuccessfully_WhenProductIsValid();
        }
    }
}
