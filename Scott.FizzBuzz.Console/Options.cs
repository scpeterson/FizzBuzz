using CommandLine;

namespace Scott.FizzBuzz.Console;

public class Options
{
    [Option('m', "method", Required = true, HelpText = "Method name, either 'imperative' or 'no-dependency'.")]
    public string Method { get; set; }
}