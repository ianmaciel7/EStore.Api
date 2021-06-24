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
    public class SubCategoriesControllerTests : IntegrationTest
    {
        private CategoryInputModel _categoryInputModel;
        private SubCategoryInputModel _subCategoryInputModel;

        public SubCategoriesControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper) : base(factory, outputHelper)
        {
        }

        [Fact]
        public async Task Register_ReturnSuccessfully_WhenSubCategoryIsValid()
        {

            _categoryInputModel = new AutoFaker<CategoryInputModel>(AutoBogusConfigurations.LOCATE);
            var content = new StringContent(JsonConvert.SerializeObject(_categoryInputModel), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync($"api/v1/Categories", content);

            _subCategoryInputModel = new AutoFaker<SubCategoryInputModel>(AutoBogusConfigurations.LOCATE);
            content = new StringContent(JsonConvert.SerializeObject(_subCategoryInputModel), Encoding.UTF8, "application/json");
            httpResponse = await _httpClient.PostAsync($"api/v1/Categories/{_categoryInputModel.Name}/SubCategories", content);

            _outputHelper.WriteLine($"{nameof(SubCategoriesControllerTests)}_{nameof(Register_ReturnSuccessfully_WhenSubCategoryIsValid)} :" + await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(System.Net.HttpStatusCode.Created, httpResponse.StatusCode);
        }

        [Fact]
        public async Task Register_ReturnFailure_WhenSubCategoryIsInvalidByNameAlreadyRegistered()
        {
            var content = new StringContent(JsonConvert.SerializeObject(_categoryInputModel), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync($"api/v1/Categories", content);
            
            content = new StringContent(JsonConvert.SerializeObject(_subCategoryInputModel), Encoding.UTF8, "application/json");
            httpResponse = await _httpClient.PostAsync($"api/v1/Categories/{_categoryInputModel.Name}/SubCategories", content);

            _outputHelper.WriteLine($"{nameof(SubCategoriesControllerTests)}_{nameof(Register_ReturnFailure_WhenSubCategoryIsInvalidByNameAlreadyRegistered)} :" + await httpResponse.Content.ReadAsStringAsync());
            Assert.Equal(System.Net.HttpStatusCode.Conflict, httpResponse.StatusCode);
        }

        public override async Task InitializeAsync()
        {
            await Register_ReturnSuccessfully_WhenSubCategoryIsValid();
        }

    }
}
