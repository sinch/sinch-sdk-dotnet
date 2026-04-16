using System.Text.Json;
using FluentAssertions;
using Sinch.Core;
using Sinch.Numbers.Active.Update;
using Xunit;

namespace Sinch.Tests.Numbers
{
    /// <summary>
    ///     Serialization tests for <see cref="UpdateActiveNumberRequest" /> covering the three
    ///     RFC 7396 JSON Merge Patch scenarios:
    ///     <list type="number">
    ///         <item>Field explicitly set to null → included in payload as <c>null</c>.</item>
    ///         <item>Field set to a value → included in payload with that value.</item>
    ///         <item>Field left unset (default) → omitted from payload entirely.</item>
    ///     </list>
    ///     Also covers <see cref="Optional{T}" /> unit behaviour.
    /// </summary>
    public class UpdateActiveNumberRequestTests : NumberTestBase
    {
        // ── Optional<T> unit tests ────────────────────────────────────────────────

        [Fact]
        public void Optional_Unset_IsUnsetType()
        {
            var sut = Optional<string>.CreateUnset();
            sut.Should().BeOfType<Optional<string>.Unset>();
        }

        [Fact]
        public void Optional_Null_IsNullType()
        {
            var sut = Optional<string>.CreateNull();
            sut.Should().BeOfType<Optional<string>.Null>();
        }

        [Fact]
        public void Optional_Value_IsValueType()
        {
            var sut = Optional<string>.CreateValue("hello");
            sut.Should().BeOfType<Optional<string>.Value>();
            ((Optional<string>.Value)sut).Data.Should().Be("hello");
        }

        // ── Serialization: Scenario "Value set" ──────────────────────────────────

        [Fact]
        public void SerializeUpdateActiveNumberRequest_WithValues_IncludesAllSetFields()
        {
            var request = new UpdateActiveNumberRequest
            {
                DisplayName = "My DID",
                SmsConfiguration = new Sinch.Numbers.SmsConfiguration { ServicePlanId = "svc-plan-1" },
                CallbackUrl = "https://example.com/callback"
            };

            var json = JsonSerializer.Serialize(request, Numbers.JsonSerializerOptions);
            var expected = Helpers.LoadResources("Numbers/Active/UpdateActiveNumberRequestAllValues.json");

            Helpers.AssertJsonEqual(expected, json);
        }

        // ── Serialization: Scenario "Explicit null" ───────────────────────────────

        [Fact]
        public void SerializeUpdateActiveNumberRequest_WithExplicitNulls_IncludesNullFields()
        {
            var request = new UpdateActiveNumberRequest
            {
                DisplayName = null,
                CallbackUrl = Optional<string>.CreateNull()
            };

            var json = JsonSerializer.Serialize(request, Numbers.JsonSerializerOptions);
            var expected = Helpers.LoadResources("Numbers/Active/UpdateActiveNumberRequestExplicitNulls.json");

            Helpers.AssertJsonEqual(expected, json);
        }

        // ── Serialization: Scenario "Unset (default)" ────────────────────────────

        [Fact]
        public void SerializeUpdateActiveNumberRequest_AllUnset_ProducesEmptyObject()
        {
            var request = new UpdateActiveNumberRequest();

            var json = JsonSerializer.Serialize(request, Numbers.JsonSerializerOptions);
            var expected = Helpers.LoadResources("Numbers/Active/UpdateActiveNumberRequestAllUnset.json");

            Helpers.AssertJsonEqual(expected, json);
        }
    }
}
