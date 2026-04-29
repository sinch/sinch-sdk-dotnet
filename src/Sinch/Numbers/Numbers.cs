using System;
using System.Collections.Generic;
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
using Sinch.Numbers.EventDestinations;
using Sinch.Numbers.SinchEvents;

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

        /// <inheritdoc cref="ISinchNumbersEventDestinations"/>
        public ISinchNumbersEventDestinations EventDestinations { get; }

        /// <inheritdoc cref="INumbersSinchEvents"/>
        public INumbersSinchEvents SinchEvents { get; }

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

        /// <inheritdoc cref="ISinchNumbersActive.List(ListActiveNumbersRequest, CancellationToken)" />
        Task<ListActiveNumbersResponse> List(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.List(CancellationToken)" />
        Task<ListActiveNumbersResponse> List(CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.ListAuto(ListActiveNumbersRequest, CancellationToken)" />
        IAsyncEnumerable<ActiveNumber> ListAuto(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default);

        /// <inheritdoc cref="ISinchNumbersActive.ListAuto(CancellationToken)" />
        IAsyncEnumerable<ActiveNumber> ListAuto(CancellationToken cancellationToken = default);

        /// <summary>
        ///     For internal use, JsonSerializerOption to be utilized for serialization and deserialization of all Numbers models
        /// </summary>
        internal JsonSerializerOptions JsonSerializerOptions { get; }
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
            EventDestinations = new SinchNumbersEventDestinations(projectId, baseAddress,
                loggerFactory?.Create<ISinchNumbersEventDestinations>(), http);
            SinchEvents = new NumbersSinchEvents(http.JsonSerializerOptions,
                loggerFactory?.Create<INumbersSinchEvents>());
            JsonSerializerOptions = http.JsonSerializerOptions;
        }

        public ISinchNumbersRegions Regions { get; }

        public ISinchNumbersEventDestinations EventDestinations { get; }

        /// <inheritdoc />
        public INumbersSinchEvents SinchEvents { get; }

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
        public Task<ListActiveNumbersResponse> List(CancellationToken cancellationToken = default)
        {
            return _activeNumbers.List(cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<ActiveNumber> ListAuto(ListActiveNumbersRequest request,
            CancellationToken cancellationToken = default)
        {
            return _activeNumbers.ListAuto(request, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncEnumerable<ActiveNumber> ListAuto(CancellationToken cancellationToken = default)
        {
            return _activeNumbers.ListAuto(cancellationToken);
        }

        public JsonSerializerOptions JsonSerializerOptions { get; }
    }
}
