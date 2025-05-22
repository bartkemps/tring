# Copilot Instruction File

Always check this file for special instructions from the user before performing any actions in this workspace.

## Important Note About Terminal Commands
- Do not use `dotnet restore && dotnet build`  in the terminal.
- Never use `&&` to chain commands in the terminal. PowerShell does not support `&&` as a statement separator. Use separate commands or `;` instead.

## Terminal Usage
- Remember that the terminal may be running PowerShell. The `&&` operator does not behave the same as in bash; use `;` or separate commands instead.

## Unit Test Guidelines
- Use xUnit, Moq.AutoMock, AutoFixture, FluentAssertions
- AutoMocker is injected.
- AutoMocker and AutoFixture may not be needed if the system under test is not a class. 
- Name tests using the pattern: `MethodName_ExpectedResult` or `MethodName_ExpectedResult_WhenConditions`.
- Arrange-Act-Assert (AAA) pattern should be followed in each test. However, we don't use comments. Instead, separate the sections with blank lines.
- No other blank lines should be present in the test method.
- No variables are used to contain the mocks. Instead, use `mocker.GetMock<>()` every time.
- Optimize for readability, not performance. (unless performance is the goal of the test or in extreme cases)

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
