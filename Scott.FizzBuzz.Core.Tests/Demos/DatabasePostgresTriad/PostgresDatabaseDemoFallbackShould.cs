using FluentAssertions;
using LanguageExt.UnitTesting;
using Scott.FizzBuzz.Core.Demos.DatabasePostgresTriad;

namespace Scott.FizzBuzz.Core.Tests.Demos.DatabasePostgresTriad;

public class PostgresDatabaseDemoFallbackShould
{
    [Fact]
    public void ImperativeDemoShouldNotThrowWhenConnectionStringIsMissing()
    {
        PostgresTestEnvironment.WithTempConnectionString(null, () =>
        {
            var demo = new ImperativePostgresDatabaseDemo();
            Action act = () => _ = demo.Run("Scott", "21");
            act.Should().NotThrow();
        });
    }

    [Fact]
    public void CSharpAndLanguageExtDemoShouldReturnLeftWhenConnectionStringIsMissing()
    {
        PostgresTestEnvironment.WithTempConnectionString(null, () =>
        {
            new CSharpFunctionalPostgresDatabaseDemo().Run("Scott", "21").ShouldBeLeft();
            new LanguageExtEffPostgresDatabaseDemo().Run("Scott", "21").ShouldBeLeft();
        });
    }
}
