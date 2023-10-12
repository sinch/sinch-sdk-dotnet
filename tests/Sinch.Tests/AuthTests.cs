using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
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
        private readonly ILoggerAdapter<Auth.OAuth> _logger = Substitute.For<ILoggerAdapter<Auth.OAuth>>();
        private readonly MockHttpMessageHandler _messageHandlerMock = new();
        private readonly MockedRequest _mockedRequest;
        private readonly IAuth _auth;

        public AuthTests()
        {
            var httpClient = new HttpClient(_messageHandlerMock);
            const string mockKeyId = "mock_key_id";
            const string mockKeySecret = "mock_key_secret";
            _auth = new Auth.OAuth(mockKeyId, mockKeySecret, httpClient, _logger);
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

            var token = await _auth.GetToken();

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

            var token = await _auth.GetToken();

            _messageHandlerMock.GetMatchCount(_mockedRequest).Should().Be(1);
            token.Should().Be("token_1");

            _mockedRequest.Respond(JsonContent.Create(new
            {
                access_token = "token_2",
                expires_in = 20,
                scope = "",
                token_type = "bearer"
            }));

            var token2 = await _auth.GetToken();
            _messageHandlerMock.GetMatchCount(_mockedRequest).Should().Be(1);
            token2.Should().Be("token_1");
        }

        [Fact]
        public async Task RenewToken()
        {
            _mockedRequest.Respond(JsonContent.Create(new
            {
                access_token = "token_1",
                expires_in = 1,
                scope = "",
                token_type = "bearer"
            }));

            var token = await _auth.GetToken();

            token.Should().Be("token_1");

            Thread.Sleep(1000);

            _mockedRequest.Respond(JsonContent.Create(new
            {
                access_token = "token_2",
                expires_in = 10,
                scope = "",
                token_type = "bearer"
            }));
            var token2 = await _auth.GetToken();
            _messageHandlerMock.GetMatchCount(_mockedRequest).Should().Be(2);
            token2.Should().Be("token_2");
        }

        [Fact]
        public async Task ThrowInvalidRequest()
        {
            _mockedRequest.Respond(HttpStatusCode.NotFound);

            Func<Task<string>> act = () => _auth.GetToken();

            await act.Should().ThrowAsync<AuthException>();
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

            Func<Task<string>> act = () => _auth.GetToken();

            await act.Should().ThrowAsync<AuthException>().Where(x =>
                x.Error == "invalid_request"
                && x.ErrorVerbose == "long_description"
                && x.ErrorDescription == "super_long_description"
                && x.ErrorHint == "how_to_fix"
            );
        }
    }
}
