using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Sinch.Auth;
using Sinch.Logger;
using Xunit;

namespace Sinch.Tests
{
    public class AuthTests
    {
        private readonly ILoggerAdapter<OAuth> _logger = Substitute.For<ILoggerAdapter<OAuth>>();
        private readonly MockHttpMessageHandler _messageHandlerMock = new();
        private readonly MockedRequest _mockedRequest;
        private readonly ISinchAuth _auth;

        public AuthTests()
        {
            var httpClient = new HttpClient(_messageHandlerMock);
            const string mockKeyId = "mock_key_id";
            const string mockKeySecret = "mock_key_secret";
            _auth = new OAuth(mockKeyId, mockKeySecret, httpClient, _logger, new Uri("https://auth.sinch.com/"));
            var basicAuthHeaderValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{mockKeyId}:{mockKeySecret}"));
            _mockedRequest = _messageHandlerMock.When(HttpMethod.Post, "https://auth.sinch.com/oauth2/token")
                .WithFormData(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                })
                .WithHeaders("Authorization", $"Basic {basicAuthHeaderValue}");
        }

        [Fact]
        public async Task FetchNewToken()
        {
            _mockedRequest.Respond(JsonContent.Create(new
            {
                access_token = "token_1",
                expires_in = 20,
                scope = "",
                token_type = "bearer"
            }));

            var token = await _auth.GetAuthToken();

            _messageHandlerMock.GetMatchCount(_mockedRequest).Should().Be(1);
            token.Should().Be("token_1");
        }

        [Fact]
        public async Task ReturnCachedToken()
        {
            _mockedRequest.Respond(JsonContent.Create(new
            {
                access_token = "token_1",
                expires_in = 20,
                scope = "",
                token_type = "bearer"
            }));

            var token = await _auth.GetAuthToken();

            _messageHandlerMock.GetMatchCount(_mockedRequest).Should().Be(1);
            token.Should().Be("token_1");

            _mockedRequest.Respond(JsonContent.Create(new
            {
                access_token = "token_2",
                expires_in = 20,
                scope = "",
                token_type = "bearer"
            }));

            var token2 = await _auth.GetAuthToken();
            _messageHandlerMock.GetMatchCount(_mockedRequest).Should().Be(1);
            token2.Should().Be("token_1");
        }

        [Fact]
        public async Task ThrowInvalidRequest()
        {
            _mockedRequest.Respond(HttpStatusCode.NotFound);

            Func<Task<string>> act = () => _auth.GetAuthToken();

            await act.Should().ThrowAsync<SinchAuthException>();
        }

        [Fact]
        public async Task ThrowInvalidRequestWithContent()
        {
            _mockedRequest.Respond(HttpStatusCode.BadRequest, JsonContent.Create(new
            {
                error = "invalid_request",
                error_verbose = "long_description",
                error_description = "super_long_description",
                error_hint = "how_to_fix"
            }));

            Func<Task<string>> act = () => _auth.GetAuthToken();

            await act.Should().ThrowAsync<SinchAuthException>().Where(x =>
                x.Error == "invalid_request"
                && x.ErrorVerbose == "long_description"
                && x.ErrorDescription == "super_long_description"
                && x.ErrorHint == "how_to_fix"
            );
        }

        [Fact]
        public void GenerateCorrectApplicationSignature()
        {
            var auth = new ApplicationSignedAuth("669E367E-6BBA-48AB-AF15-266871C28135", "BeIukql3pTKJ8RGL5zo0DA==");

            var json = "{\"identity\": {\"type\": \"number\", \"endpoint\": \"+46700000000\"}, \"method\": \"sms\"}";

            var result = auth.GetSignedAuth(Encoding.UTF8.GetBytes(json), "POST", "/verification/v1/verifications",
                "x-timestamp:2014-06-04T13:41:58Z",
                "application/json");

            result.Should()
                .BeEquivalentTo("669E367E-6BBA-48AB-AF15-266871C28135:h6rXrFayOoggyHW4ymnLlfSDkZZPg6j98lHzvOXVjvw=");
        }

        [Fact]
        public void AppSignatureWithEmptyBody()
        {
            var auth = new ApplicationSignedAuth("669E367E-6BBA-48AB-AF15-266871C28135", "BeIukql3pTKJ8RGL5zo0DA==");

            var result = auth.GetSignedAuth(null, "POST", "/verification/v1/verifications",
                "x-timestamp:2014-06-04T13:41:58Z",
                "application/json");

            result.Should()
                .BeEquivalentTo("669E367E-6BBA-48AB-AF15-266871C28135:srx3SkKXw/ryFJLfwFFzZTigPxR/1+9Ae+eB3olDIjM=");
        }
    }
}
