# Copilot Instruction File

Always check this file for special instructions from the user before performing any actions in this workspace.

## Important Note About Terminal Commands
- Do not use `dotnet restore && dotnet build`  in the terminal.
- Never use `&&` to chain commands in the terminal. PowerShell does not support `&&` as a statement separator. Use separate commands or `;` instead.

## Terminal Usage
- Remember that the terminal may be running PowerShell. The `&&` operator does not behave the same as in bash; use `;` or separate commands instead.

## Code Style and Implementation Preferences
- Prefer simple, direct code over complex abstractions
- String interpolation is often preferred over StringBuilder for readable string formatting
- Careful with implementations that use multiple lines in DebuggerDisplay attributes - they don't render well in some IDEs like Rider
- When existing code has two separate methods (e.g., ToString() for full output and DebugView() for debugger-friendly display), respect this pattern
- Always thoroughly examine existing code before proposing changes - don't replace working implementations with more complex ones
- Performance matters - avoid unnecessary allocations and iterations where simple alternatives exist
- Prefer raw string literals (""" """) for multi-line strings when clarity is important

## Unit Test Guidelines
- Use xUnit, Moq.AutoMock, AutoFixture, FluentAssertions
- AutoMocker is injected.
- AutoMocker and AutoFixture may not be needed if the system under test is not a class. 
- Name tests using the pattern: `MethodName_ExpectedResult` or `MethodName_ExpectedResult_WhenConditions`.
- Arrange-Act-Assert (AAA) pattern should be followed in each test. However, we don't use comments. Instead, separate the sections with blank lines.
- No other blank lines should be present in the test method.
- No variables are used to contain the mocks. Instead, use `mocker.GetMock<>()` every time.
- Optimize for readability, not performance. (unless performance is the goal of the test or in extreme cases)

## BinaryOperationBuilder Test Guidelines
- For testing BinaryOperationBuilder with different types:
  - Create separate test methods for each type, named like `Build_GeneratesOperation_ForUInt32`, `Build_GeneratesOperation_ForUInt64`, etc.
  - Use `[Theory]` with multiple `[InlineData]` entries containing only input values, not expected results
  - Calculate expected results within the test using operations on Trit objects
  - Use variable names "actual" and "expected" for result comparison
  - Include descriptive assertion messages like `$"because {trit1} XOR {trit2} should equal {expected}"`
  - When testing with different numeric types, include the type in variable names (e.g., "negativeUint1", "positiveUlong2")

## Example Test

Here is an example of a well-structured unit test using xUnit, Moq.AutoMock, AutoFixture, and FluentAssertions:

```csharp
[Theory]
[AutoData]
public void Flip_FlipsCoin(
    AutoMocker mocker,
    Coin coin
    )
{
    mocker
        .GetMock<IRepatService>()
        .Setup(s => s.GetRepeatCount(It.IsAny<Coin>()))
        .Returns(1);
    mocker
        .GetMock<IRepatService>()
        .Setup(s => s.GetRepeatDelay())
        .Returns(1);
    var sut = new mocker.CreateInstance<CoinFlipper>();
    coin.CurrentSide = CoinSide.Tails;

    var actual = sut.Flip(coin);
}
```

## Example BinaryOperationBuilder Test

```csharp
[Theory]
[InlineData(-1, -1)] 
[InlineData(-1, 0)]
[InlineData(-1, 1)]
[InlineData(0, -1)] 
[InlineData(0, 0)]
[InlineData(0, 1)] 
[InlineData(1, -1)] 
[InlineData(1, 0)]
[InlineData(1, 1)] 
public void Build_GeneratesOperation_ForUInt32(sbyte trit1Value, sbyte trit2Value)
{
    var xor = new TritLookupTable(
        null, false, true,
        false, null, true,
        true, true, null
    );
    var builder = new BinaryOperationBuilder<uint>(xor);
    var operation = builder.Build();
    var trit1 = new Trit(trit1Value);
    var trit2 = new Trit(trit2Value);
    var expected = (trit1 | xor | trit2);
    var negative1 = trit1Value == -1 ? 1u : 0u;
    var positive1 = trit1Value == 1 ? 1u : 0u;
    var negative2 = trit2Value == -1 ? 1u : 0u;
    var positive2 = trit2Value == 1 ? 1u : 0u;

    operation(negative1, positive1, negative2, positive2, out uint negativeResult, out uint positiveResult);

    var actual = TritConverter.GetTrit(ref positiveResult, ref negativeResult, 0);
    actual.Value.Should().Be(expected.Value, $"because {trit1} XOR {trit2} should equal {expected}");
}
```
