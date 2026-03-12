namespace Scott.FizzBuzz.Core.Demos.EndToEndMiniFeatureTriad;

public static class CSharpFunctionalRegistrationLogic
{
    public static RegistrationResult<RegisteredUser> Register(string? name, string? ageRaw) =>
        ValidateName(name)
            .Bind(validName => ParseAge(ageRaw)
                .Bind(RequireAdult)
                .Map(age => new RegistrationRequest(validName, age)))
            .Bind(Save);

    public static RegistrationResult<string> ValidateName(string? value) =>
        !string.IsNullOrWhiteSpace(value)
            ? RegistrationResult<string>.Success(value.Trim())
            : RegistrationResult<string>.Failure("Name is required.");

    public static RegistrationResult<int> ParseAge(string? value) =>
        int.TryParse(value, out var parsed)
            ? RegistrationResult<int>.Success(parsed)
            : RegistrationResult<int>.Failure("Age must be numeric.");

    public static RegistrationResult<int> RequireAdult(int age) =>
        age >= 18
            ? RegistrationResult<int>.Success(age)
            : RegistrationResult<int>.Failure("Must be at least 18.");

    public static RegistrationResult<RegisteredUser> Save(RegistrationRequest request) =>
        RegistrationResult<RegisteredUser>.Success(new RegisteredUser(
            $"{request.Name.ToLowerInvariant()}-{request.Age}",
            request.Name,
            request.Age));

    public readonly record struct RegistrationResult<T>(bool IsSuccess, T? Value, string? Error)
    {
        public static RegistrationResult<T> Success(T value) => new(true, value, null);
        public static RegistrationResult<T> Failure(string error) => new(false, default, error);

        public RegistrationResult<TNext> Bind<TNext>(Func<T, RegistrationResult<TNext>> next) =>
            IsSuccess && Value is not null
                ? next(Value)
                : RegistrationResult<TNext>.Failure(Error ?? "Registration failed.");

        public RegistrationResult<TNext> Map<TNext>(Func<T, TNext> map) =>
            IsSuccess && Value is not null
                ? RegistrationResult<TNext>.Success(map(Value))
                : RegistrationResult<TNext>.Failure(Error ?? "Registration failed.");
    }
}
