using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Sinch.Core;
using Sinch.Logger;
using Sinch.Numbers.Active;
using Sinch.Numbers.Active.List;
using Sinch.Numbers.Active.Update;
using Sinch.Numbers.Available;
using Sinch.Numbers.Available.List;
using Sinch.Numbers.Available.Rent;
using Sinch.Numbers.Available.RentAny;
using Sinch.Numbers.Callbacks;

namespace Sinch.Numbers
{
    /// <summary>
    ///     You can use the Active Number API to manage numbers you own. Assign numbers to projects, release numbers from
    ///     projects, or list all numbers assigned to a project.
    /// </summary>
    public interface ISinchNumbers
    {
        /// <summary>
        ///     You can use the Available Regions API to list all the regions that have numbers assigned to a project.
        /// </summary>
        public ISinchNumbersRegions Regions { get; }

        /// <inheritdoc cref="ISinchNumbersCallbacks"/>
        public ISinchNumbersCallbacks Callbacks { get; }

        /// <inheritdoc cref="ISinchNumbersAvailable.RentAny" />
        Task<ActiveNumber> RentAny(RentAnyNumberRequest request,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersAvailable.Rent" />
        Task<ActiveNumber> Rent(string phoneNumber,
            RentActiveNumberRequest request, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersAvailable.List" />
        Task<ListAvailableNumbersResponse> SearchForAvailableNumbers(
            ListAvailableNumbersRequest request, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersAvailable.CheckAvailability" />
        Task<AvailableNumber> CheckAvailability(string phoneNumber,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.Release" />
        Task<ActiveNumber> Release(
            string phoneNumber, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.Get" />
        Task<ActiveNumber> Get(string phoneNumber,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.Update" />
        Task<ActiveNumber> Update(string phoneNumber,
            UpdateActiveNumberRequest request, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.List" />
        Task<ListActiveNumbersResponse> List(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.ListAuto" />
        IAsyncEnumerable<ActiveNumber> ListAuto(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     For internal use, JsonSerializerOption to be utilized for serialization and deserialization of all Numbers models
        /// </summary>
        internal JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>
        ///     Validates json of a Webhook event with your HMAC secret 
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret</param>
        /// <param name="json">The JSON payload as a raw string to be validated.</param>
        /// <param name="signatureHeaderValue">A value of X-Sinch-Signature header</param>
        /// <returns>True if a validation is successful</returns>
        bool ValidateAuthenticationHeader(string hmacSecret, string json, string signatureHeaderValue);

        /// <summary>
        ///     Validates json of a Webhook event with your HMAC secret 
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret</param>
        /// <param name="json">The JSON payload as a raw string to be validated.</param>
        /// <param name="headers">Headers of a Webhook message, where method will look up for X-Sinch-Signature header</param>
        /// <returns></returns>
        bool ValidateAuthenticationHeader(string hmacSecret, string json, HttpHeaders headers);
    }

    public sealed class Numbers : ISinchNumbers
    {
        private readonly ISinchNumbersActive _activeNumbers;
        private readonly ISinchNumbersAvailable _available;
        internal Numbers(string projectId, Uri baseAddress,
            LoggerFactory? loggerFactory, IHttp http)
        {
            Regions = new AvailableRegions(projectId, baseAddress,
                loggerFactory?.Create<AvailableRegions>(), http);
            _activeNumbers = new ActiveNumbers(projectId, baseAddress,
                loggerFactory?.Create<ActiveNumbers>(), http);
            _available = new AvailableNumbers(projectId, baseAddress,
                loggerFactory?.Create<AvailableNumbers>(), http);
            Callbacks = new SinchNumbersCallbacks(projectId, baseAddress,
                loggerFactory?.Create<ISinchNumbersCallbacks>(), http);
            JsonSerializerOptions = http.JsonSerializerOptions;
        }

        public ISinchNumbersRegions Regions { get; }

        public ISinchNumbersCallbacks Callbacks { get; }

        /// <inheritdoc />
        public Task<ActiveNumber> RentAny(RentAnyNumberRequest request, CancellationToken cancellationToken = default)
        {
            return _available.RentAny(request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Rent(string phoneNumber, RentActiveNumberRequest request,
            CancellationToken cancellationToken = default)
        {
            return _available.Rent(phoneNumber, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListAvailableNumbersResponse> SearchForAvailableNumbers(ListAvailableNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            return _available.List(request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<AvailableNumber> CheckAvailability(string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return _available.CheckAvailability(phoneNumber, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Release(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return _activeNumbers.Release(phoneNumber, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Get(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return _activeNumbers.Get(phoneNumber, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Update(string phoneNumber, UpdateActiveNumberRequest request,
            CancellationToken cancellationToken = default)
        {
            return _activeNumbers.Update(phoneNumber, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListActiveNumbersResponse> List(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            return _activeNumbers.List(request, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<ActiveNumber> ListAuto(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            return _activeNumbers.ListAuto(request, cancellationToken);
        }

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public bool ValidateAuthenticationHeader(string hmacSecret, string json, string signatureHeaderValue)
        {
            return HeaderValidation.ValidateAuthHeader(hmacSecret, json, signatureHeaderValue);
        }

        public bool ValidateAuthenticationHeader(string hmacSecret, string json, HttpHeaders headers)
        {
            return HeaderValidation.ValidateAuthHeader(hmacSecret, json, headers);
        }
    }
}
