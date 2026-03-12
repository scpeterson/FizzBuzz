using LanguageExt;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Console;

public class DemoRunner
{
    private const int DescriptionWrapWidth = 100;

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

    public DemoExecutionResult Execute(Options opts)
    {
        if (opts is null)
        {
            return DemoExecutionResult.Failure($"{nameof(opts)} was null");
        }

        var contractValidation = ValidateContract(opts);
        if (!contractValidation.IsSuccess)
        {
            return contractValidation;
        }

        if (opts.List)
        {
            ShowAvailableDemos(opts.Tags);
            return DemoExecutionResult.Success();
        }

        if (string.IsNullOrWhiteSpace(opts.Method))
        {
            return DemoExecutionResult.Failure("No method specified");
        }

        return _demos.TryGetValue(opts.Method, out var demo)
            ? demo.Run(opts.Name, opts.Number)
            : DemoExecutionResult.Failure($"Unknown demo \"{opts.Method}\"");
    }

    private static DemoExecutionResult ValidateContract(Options options)
    {
        var hasMethod = !string.IsNullOrWhiteSpace(options.Method);
        var hasName = !string.IsNullOrWhiteSpace(options.Name);
        var hasNumber = !string.IsNullOrWhiteSpace(options.Number);
        var hasTags = options.Tags?.Any(tag => !string.IsNullOrWhiteSpace(tag)) ?? false;

        if (hasTags && !options.List)
        {
            return DemoExecutionResult.Failure("--tag can only be used with --list.");
        }

        if (options.List && hasMethod)
        {
            return DemoExecutionResult.Failure("--method cannot be combined with --list.");
        }

        if (options.List && (hasName || hasNumber))
        {
            return DemoExecutionResult.Failure("--name/--number cannot be combined with --list.");
        }

        return DemoExecutionResult.Success();
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
            .OrderBy(GetLearningStage)
            .ThenBy(GetCategoryRank)
            .ThenBy(demo => demo.Key)
            .ToList();

        if (demos.Count == 0)
        {
            WriteLine("No demos match the supplied filters.");
            return;
        }

        int? currentStage = null;

        for (var index = 0; index < demos.Count; index++)
        {
            var demo = demos[index];
            var tagOutput = demo.Tags.Count == 0 ? "none" : string.Join(",", demo.Tags);
            var learningStage = GetLearningStage(demo);

            if (currentStage != learningStage)
            {
                if (index > 0)
                {
                    WriteLine(string.Empty);
                }

                WriteLine($"== {GetLearningStageHeader(learningStage)} ==");
                WriteLine(string.Empty);
                currentStage = learningStage;
            }
            else if (index > 0)
            {
                WriteLine(string.Empty);
            }

            WriteLine(demo.Key);
            WriteLine($"  category: {demo.Category}");
            WriteLine($"  tags: {tagOutput}");

            if (!string.IsNullOrWhiteSpace(demo.Description))
            {
                foreach (var line in WrapDescription(demo.Description))
                {
                    WriteLine(line);
                }
            }
        }
    }

    private void WriteLine(string message) => _output.WriteLine(message);

    private static int GetLearningStage(IDemo demo)
    {
        if (demo.Tags.Contains("baseline", StringComparer.OrdinalIgnoreCase))
            return demo.Tags.Contains("supporting-feature", StringComparer.OrdinalIgnoreCase) ? 0 : 1;

        if (demo.Tags.Contains("comparison", StringComparer.OrdinalIgnoreCase))
            return 2;

        if (demo.Tags.Contains("dotnet10", StringComparer.OrdinalIgnoreCase) ||
            demo.Tags.Contains("csharp14", StringComparer.OrdinalIgnoreCase))
            return 3;

        return 4;
    }

    private static string GetLearningStageHeader(int learningStage) => learningStage switch
    {
        0 => "Supporting C# Features",
        1 => "Core Baseline Demos",
        2 => "Triad Comparisons",
        3 => ".NET 10 / C# 14 FP Features",
        _ => "Advanced Functional Topics"
    };

    private static IReadOnlyList<string> WrapDescription(string description)
    {
        const string firstPrefix = "  description: ";
        const string continuationPrefix = "               ";

        var words = description
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (words.Length == 0)
        {
            return [$"{firstPrefix}{description}"];
        }

        var lines = new List<string>();
        var currentLine = firstPrefix;

        foreach (var word in words)
        {
            var separator = currentLine == firstPrefix || currentLine == continuationPrefix ? string.Empty : " ";
            if (currentLine.Length + separator.Length + word.Length > DescriptionWrapWidth)
            {
                lines.Add(currentLine);
                currentLine = continuationPrefix + word;
                continue;
            }

            currentLine += separator + word;
        }

        lines.Add(currentLine);
        return lines;
    }

    private static int GetCategoryRank(IDemo demo) => demo.Category switch
    {
        "imperative" => 0,
        "csharp-support" => 1,
        "csharp" => 2,
        "functional" => 3,
        "general" => 4,
        _ => 5
    };
}
