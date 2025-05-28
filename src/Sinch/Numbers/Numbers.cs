using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
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

        /// <summary>
        ///     You can use the Available Number API to search for available numbers or activate an available number.
        /// </summary>
        [Obsolete($"This property is obsolete, use methods of this ({nameof(ISinchNumbers)}) interface instead.")]
        public ISinchNumbersAvailable Available { get; }

        /// <summary>
        ///     You can use the Active Number API to manage numbers you own. Assign numbers to projects,
        ///     release numbers from projects, or list all numbers assigned to a project.
        /// </summary>
        [Obsolete($"This property is obsolete, use methods of this ({nameof(ISinchNumbers)}) interface instead.")]
        public ISinchNumbersActive Active { get; }

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
        /// <param name="json">Json body to validate</param>
        /// <param name="signatureHeaderValue">A value of X-Sinch-Signature header</param>
        /// <returns>True if a validation is successful</returns>
        bool ValidateAuthHeader(string hmacSecret, string json, string signatureHeaderValue);

        /// <summary>
        ///     Validates json of a Webhook event with your HMAC secret 
        /// </summary>
        /// <param name="hmacSecret">Your HMAC secret</param>
        /// <param name="json">Json body to validate</param>
        /// <param name="headers">Headers of a Webhook message, where method will look up for X-Sinch-Signature header</param>
        /// <returns></returns>
        bool ValidateAuthHeader(string hmacSecret, string json, HttpHeaders headers);


    }

    public sealed class Numbers : ISinchNumbers
    {
        internal Numbers(string projectId, Uri baseAddress,
            LoggerFactory? loggerFactory, IHttp http)
        {
            Regions = new AvailableRegions(projectId, baseAddress,
                loggerFactory?.Create<AvailableRegions>(), http);
            Active = new ActiveNumbers(projectId, baseAddress,
                loggerFactory?.Create<ActiveNumbers>(), http);
            Available = new AvailableNumbers(projectId, baseAddress,
                loggerFactory?.Create<AvailableNumbers>(), http);
            Callbacks = new SinchNumbersCallbacks(projectId, baseAddress,
                loggerFactory?.Create<ISinchNumbersCallbacks>(), http);
            JsonSerializerOptions = http.JsonSerializerOptions;
        }

        public ISinchNumbersRegions Regions { get; }

        public ISinchNumbersActive Active { get; }

        public ISinchNumbersAvailable Available { get; }

        public ISinchNumbersCallbacks Callbacks { get; }

        // disabling obsolete usage as in next major version, active and available interfaces will remain,
        // but visibility changed to internal, and public interface will be available only through this methods
#pragma warning disable CS0618 // Type or member is obsolete
        /// <inheritdoc />
        public Task<ActiveNumber> RentAny(RentAnyNumberRequest request, CancellationToken cancellationToken = default)
        {
            return Available.RentAny(request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Rent(string phoneNumber, RentActiveNumberRequest request,
            CancellationToken cancellationToken = default)
        {
            return Available.Rent(phoneNumber, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListAvailableNumbersResponse> SearchForAvailableNumbers(ListAvailableNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            return Available.List(request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<AvailableNumber> CheckAvailability(string phoneNumber,
            CancellationToken cancellationToken = default)
        {
            return Available.CheckAvailability(phoneNumber, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Release(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return Active.Release(phoneNumber, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Get(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return Active.Get(phoneNumber, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ActiveNumber> Update(string phoneNumber, UpdateActiveNumberRequest request,
            CancellationToken cancellationToken = default)
        {
            return Active.Update(phoneNumber, request, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ListActiveNumbersResponse> List(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            return Active.List(request, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<ActiveNumber> ListAuto(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            return Active.ListAuto(request, cancellationToken);
        }


#pragma warning restore CS0618 // Type or member is obsolete

        public JsonSerializerOptions JsonSerializerOptions { get; }

        public bool ValidateAuthHeader(string hmacSecret, string json, string signatureHeaderValue)
        {
            if (string.IsNullOrEmpty(hmacSecret) || string.IsNullOrEmpty(signatureHeaderValue) ||
                string.IsNullOrEmpty(json))
            {
                return false;
            }

            var result = ComputeHmacSha1(hmacSecret, json);
            return string.Equals(signatureHeaderValue, result);
        }

        public bool ValidateAuthHeader(string hmacSecret, string json, HttpHeaders headers)
        {
            var headersNormalized = headers.ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);
            if (!headersNormalized.TryGetValue("x-sinch-signature", out var signature))
            {
                return false;
            }

            var signatureValue = signature.FirstOrDefault();
            if (string.IsNullOrEmpty(signatureValue))
            {
                return false;
            }

            return ValidateAuthHeader(hmacSecret, json, signatureValue);
        }

        private static string ComputeHmacSha1(string secret, string body)
        {
            using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(body));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
