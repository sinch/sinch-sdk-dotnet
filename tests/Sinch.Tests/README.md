# Run Tests

- Run without e2e tests

```bash
dotnet test --filter FullyQualifiedName!~e2e
```

### e2e

See `e2e/TestBase.cs` for required environment variables.

#### Tests naming convention 

Use request id and test file name to identify manual mock mapping.

Test in `e2e/Conversations/ConversationsTest.cs` with name `InjectEvent` will be mapped to mock id `Conversations/InjectEvent` in doppelganger.

Name tests based on Open Api Spec mocks to `Oas{TestName}`
