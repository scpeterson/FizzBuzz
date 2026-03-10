using System.Text.Json;
using System.Text.Json.Serialization;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.DotNet10Features;

public static class FpJsonStrictValidationLogic
{
    public static Either<Error, CreateUserRequest> ParseRequestStrict(string json) =>
        Try(() => JsonSerializer.Deserialize<CreateUserRequest>(json, JsonSerializerOptions.Strict))
            .ToEither(ex => Error.New($"Strict JSON parse failed: {ex.Message}"))
            .Bind(request => request is null
                ? Left<Error, CreateUserRequest>(Error.New("Payload was null after deserialization."))
                : Right<Error, CreateUserRequest>(request));

    public static Validation<Error, AdultUser> ValidateAdult(CreateUserRequest request) =>
        Success<Error, Func<string, int, AdultUser>>((name, age) => new AdultUser(name, age))
            .Apply(ValidateName(request.Name))
            .Apply(ValidateAge(request.Age));

    public static Validation<Error, string> ValidateName(string name) =>
        !string.IsNullOrWhiteSpace(name)
            ? Success<Error, string>(name)
            : Fail<Error, string>(Error.New("Name is required."));

    public static Validation<Error, int> ValidateAge(int age) =>
        age >= 18
            ? Success<Error, int>(age)
            : Fail<Error, int>(Error.New($"Age must be >= 18 but was {age}."));
}

public sealed record CreateUserRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("age")] int Age);

public sealed record AdultUser(string Name, int Age);
