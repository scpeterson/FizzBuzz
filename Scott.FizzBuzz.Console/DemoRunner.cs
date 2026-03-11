using LanguageExt;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Console;

public class DemoRunner
{
    // Keep both structures intentionally:
    // - _allDemos supports list/filter/sort views without rebuilding a sequence.
    // - _demos provides O(1) key lookup for method execution.
    private readonly IReadOnlyList<IDemo> _allDemos;
    private readonly Dictionary<string, IDemo> _demos;
    private readonly IOutput _output;

    // Option A: DI version
    public DemoRunner(IEnumerable<IDemo> demos) : this(demos, new ConsoleOutput())
    {
    }

    public DemoRunner(IEnumerable<IDemo> demos, IOutput output)
    {
        // Functional null‑guard using LanguageExt:
        var nonNullDemos =
            Optional(demos)
                .ToEither($"{nameof(demos)} was null")
                .Match(
                    Right: ds => ds,
                    Left: msg => throw new ArgumentNullException(nameof(demos), msg)
                );
        
        _allDemos = nonNullDemos.ToList();
        var duplicateKeys = _allDemos
            .GroupBy(d => d.Key)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicateKeys.Count > 0)
        {
            throw new ArgumentException(
                $"Duplicate demo key(s): {string.Join(", ", duplicateKeys)}",
                nameof(demos));
        }

        _demos = _allDemos.ToDictionary(d => d.Key, d => d);
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    public Either<string, Unit> Execute(Options opts)
        => Optional(opts)
            .ToEither($"{nameof(opts)} was null")
            .Bind<Unit>(options =>
            {
                var contractValidation = ValidateContract(options);
                if (contractValidation.IsLeft)
                {
                    return contractValidation;
                }

                if (options.List)
                {
                    ShowAvailableDemos(options.Tags);
                    return unit;
                }

                return Optional(options.Method)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToEither("No method specified")
                    .Bind<Unit>(method =>
                        _demos.TryGetValue(method, out var demo)
                            ? demo.Run(options.Name, options.Number)
                            : Left<string, Unit>($"Unknown demo \"{method}\"")
                    );
            });

    private static Either<string, Unit> ValidateContract(Options options)
    {
        var hasMethod = !string.IsNullOrWhiteSpace(options.Method);
        var hasName = !string.IsNullOrWhiteSpace(options.Name);
        var hasNumber = !string.IsNullOrWhiteSpace(options.Number);
        var hasTags = options.Tags?.Any(tag => !string.IsNullOrWhiteSpace(tag)) ?? false;

        if (hasTags && !options.List)
        {
            return Left<string, Unit>("--tag can only be used with --list.");
        }

        if (options.List && hasMethod)
        {
            return Left<string, Unit>("--method cannot be combined with --list.");
        }

        if (options.List && (hasName || hasNumber))
        {
            return Left<string, Unit>("--name/--number cannot be combined with --list.");
        }

        return unit;
    }

    private void ShowAvailableDemos(IEnumerable<string>? tags)
    {
        var normalizedTags = tags?
            .Where(tag => !string.IsNullOrWhiteSpace(tag))
            .Select(tag => tag.Trim().ToLowerInvariant())
            .Distinct()
            .ToList() ?? [];

        var demos = _allDemos
            .Where(demo => normalizedTags.Count == 0 ||
                           normalizedTags.All(tag =>
                               demo.Tags.Any(demoTag =>
                                   string.Equals(demoTag, tag, StringComparison.OrdinalIgnoreCase))))
            .OrderBy(demo => demo.Category)
            .ThenBy(demo => demo.Key)
            .ToList();

        if (demos.Count == 0)
        {
            WriteLineEff("No demos match the supplied filters.").Run();
            return;
        }

        foreach (var demo in demos)
        {
            var tagOutput = demo.Tags.Count == 0 ? "none" : string.Join(",", demo.Tags);
            var descriptionOutput = string.IsNullOrWhiteSpace(demo.Description)
                ? string.Empty
                : $" | description={demo.Description}";
            WriteLineEff($"{demo.Key} | category={demo.Category} | tags={tagOutput}{descriptionOutput}").Run();
        }
    }

    private Eff<Unit> WriteLineEff(string message) =>
        Eff(() =>
        {
            _output.WriteLine(message);
            return unit;
        });
}
