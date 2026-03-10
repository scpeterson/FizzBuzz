using LanguageExt;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.MonadFoundations;

public static class OtherMonadsValidation
{
    public static Either<string, string> CheckNotEmpty(string field, string value) =>
        !string.IsNullOrWhiteSpace(value)
            ? Right<string, string>(value)
            : Left<string, string>($"{field} is required.");

    public static Either<string, string> CheckMinLength(string field, string value, int min) =>
        value.Length >= min
            ? Right<string, string>(value)
            : Left<string, string>($"{field} must be at least {min} characters.");

    public static Either<string, (string Email, string Password)> ValidateUser(string email, string pwd) =>
        from e1 in CheckNotEmpty("Email", email)
        from e2 in CheckMinLength("Email", e1, 5)
        from p1 in CheckNotEmpty("Password", pwd)
        from p2 in CheckMinLength("Password", p1, 8)
        select (Email: e2, Password: p2);
}
