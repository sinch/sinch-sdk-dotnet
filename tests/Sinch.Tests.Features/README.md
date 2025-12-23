# Init

Checkout `sinch-sdk-mockserver` to `tests/Sinch.Tests.Features/sinch-sdk-mockserver`

## Run specific feature files
```csharp
dotnet test tests/Sinch.Tests.Features --filter FeatureTitle~[Voice]&FeatureTitle!~[Numbers][Emergency Address]
```

For more filtering, see: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test?tabs=dotnet-test-with-vstest#filter-option-details
