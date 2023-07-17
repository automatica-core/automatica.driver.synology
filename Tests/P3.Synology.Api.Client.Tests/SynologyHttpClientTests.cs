﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using P3.Synology.Api.Client.ApiDescription;
using P3.Synology.Api.Client.Exceptions;
using P3.Synology.Api.Client.Shared.Models;
using P3.Synology.Api.Client.Tests.Fixtures;
using RichardSzalay.MockHttp;
using Xunit;

namespace P3.Synology.Api.Client.Tests
{
    public class SynologyHttpClientTests : IClassFixture<SynologyFixture>, IDisposable
    {
        private readonly SynologyFixture _synologyFixture;
        private readonly Fixture _fixture;
        private readonly IApiInfo _apiInfo;
        private readonly MockHttpMessageHandler _mockHtttp;
        private readonly Uri _baseUriWithApiPath;

        public SynologyHttpClientTests(SynologyFixture synologyFixture)
        {
            _synologyFixture = synologyFixture;
            _fixture = new Fixture();
            _apiInfo = _synologyFixture.ApisInfo.InfoApi;
            _mockHtttp = new MockHttpMessageHandler();
            _baseUriWithApiPath = new Uri(_synologyFixture.BaseUri, _apiInfo.Path);
        }

        public void Dispose()
        {
            _mockHtttp.Dispose();
        }

        [Fact]
        public async Task GetAsync_ShouldCallCorrectUri()
        {
            // Arrange
            var expectedPath = _synologyFixture.BaseUrl.TrimEnd('/') + '/' + _apiInfo.Path;

            var expectedResponse = new BaseApiResponse
            {
                Success = true
            };

            //expect certain request to server
            var request = _mockHtttp.Expect(System.Net.Http.HttpMethod.Get, expectedPath)
                .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResponse));

            var httpClient = _mockHtttp.ToHttpClient();
            httpClient.BaseAddress = _synologyFixture.BaseUri;

            ISynologyHttpClient synologyHttpclient = new SynologyHttpClient(httpClient);

            // Act
            var result = await synologyHttpclient.GetAsync<BaseApiResponse>(_apiInfo, "apiMethod", new Dictionary<string, string>());

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PostAsync_ShouldCallCorrectUri()
        {
            // Arrange
            var expectedPath = _synologyFixture.BaseUrl.TrimEnd('/') + '/' + _apiInfo.Path;

            var expectedResponse = new BaseApiResponse
            {
                Success = true
            };

            //expect certain request to server
            var request = _mockHtttp.Expect(System.Net.Http.HttpMethod.Post, expectedPath)
                .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResponse));

            var httpClient = _mockHtttp.ToHttpClient();
            httpClient.BaseAddress = _synologyFixture.BaseUri;

            ISynologyHttpClient synologyHttpclient = new SynologyHttpClient(httpClient);

            // Act
            var result = await synologyHttpclient.PostAsync<BaseApiResponse>(_apiInfo, "apiMethod", null);

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetAsync_HasAdditionalErrors_ShouldReturnAdditionalErrors()
        {
            // Arrange
            var errorPath = _fixture.Create<string>();
            var expectedPath = _synologyFixture.BaseUrl.TrimEnd('/') + '/' + _apiInfo.Path;
            var additionalErrorCode = 408;

            var expectedResponse = new BaseApiResponse
            {
                Success = false,
                Error = new Error
                {
                    Code = 1002,
                    Errors = new List<Shared.Models.Errors>
                    {
                        new Shared.Models.Errors
                        {
                            Code = additionalErrorCode,
                            Path = errorPath
                        }
                    }
                }
            };

            var request = _mockHtttp.Expect(System.Net.Http.HttpMethod.Get, expectedPath)
               .Respond(HttpStatusCode.OK, JsonContent.Create(expectedResponse));

            var httpClient = _mockHtttp.ToHttpClient();
            httpClient.BaseAddress = _synologyFixture.BaseUri;

            ISynologyHttpClient synologyHttpclient = new SynologyHttpClient(httpClient);

            // Act
            var synologyApiException = await Assert.ThrowsAsync<SynologyApiException>(() => synologyHttpclient.GetAsync<BaseApiResponse>(_apiInfo, "apiMethod", new Dictionary<string, string>()));

            // Assert
            synologyApiException.Data.Count.Should().BeGreaterThan(0);
        }
    }
}
