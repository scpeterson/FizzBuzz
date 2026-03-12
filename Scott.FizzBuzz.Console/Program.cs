// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommandLine;
using Scott.FizzBuzz.Console;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Interfaces;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IOutput, ConsoleOutput>();
        services.AddFizzBuzzDemos();
        
        services.AddTransient<DemoRunner>();
    })
    .Build();

var parser = new Parser(with => with.HelpWriter = Console.Error);

var exitCode = parser.ParseArguments<Options>(args)
    .MapResult(
        (Options opts) =>
        {
            var runner = host.Services.GetRequiredService<DemoRunner>();
            var result = runner.Execute(opts);
            if (result.IsSuccess)
            {
                return 0;
            }

            Console.Error.WriteLine(result.Error);
            return 1;
        },
        errors =>
        {
            ShowParseErrors(errors);
            return 1;
        });

Environment.ExitCode = exitCode;

return;


void ShowParseErrors(IEnumerable<Error> errors)
{
    Console.Error.WriteLine("Failed to parse command line arguments due to the following errors:");
    foreach (var error in errors)
    {
        Console.Error.WriteLine($"- {error.Tag}: {error}");
    }
}
