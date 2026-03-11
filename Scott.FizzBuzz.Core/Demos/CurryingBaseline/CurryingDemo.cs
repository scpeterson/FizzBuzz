using LanguageExt;
using Scott.FizzBuzz.Core;
using Scott.FizzBuzz.Core.Currying;
using Scott.FizzBuzz.Core.Interfaces;
using static Scott.FizzBuzz.Core.OutputUtilities;
using static LanguageExt.Prelude;

namespace Scott.FizzBuzz.Core.Demos.CurryingBaseline;

public class CurryingDemo : IDemo
{
    private readonly IOutput _output;

    public CurryingDemo() : this(new ConsoleOutput())
    {
    }

    public CurryingDemo(IOutput output)
    {
        _output = output;
    }

    public const string DemoKey = "demo-currying";

    public string Key => DemoKey;
    public string Category => "csharp";
    public IReadOnlyCollection<string> Tags => ["fp", "csharp", "currying", "comparison", "baseline"];
    public Either<string, Unit> Run(string? name, string? number)
    {
        // 1) Simple curried adder
        // `Func<int, Func<int, int>>` is a function that takes an int and returns another function of type `Func<int, int>`
        // The first function takes `a` and returns a new function that takes `b` and returns the sum of `a` and `b`
        Func<int, Func<int,int>> curriedAdd = x => y => x + y;
        var add5 = curriedAdd(5);
        _output.WriteLine($"5 + 10 = {add5(10)}");

        // 2) Un‑curried vs curried filter
        Func<IEnumerable<Employee>, Func<Func<Employee,bool>, 
            IEnumerable<Employee>>> uncurriedFilter = 
            list => pred => list.Where(pred);
        
        // The outer function takes a `Func<Employee, bool>` as a parameter (this is our predicate)
        // It returns another function (Func<IEnumerable<Employee>, IEnumerable<Employee>>) that applies this
        // predicate to a sequence of Employee objects
        Func<Func<Employee,bool>, 
            Func<IEnumerable<Employee>, IEnumerable<Employee>>> curriedFilter =
            pred => list => list.Where(pred);

        var employees = SampleEmployees();

        // partial‑apply the department and age predicates
        var itFilter  = curriedFilter(e => e.Department == "IT");
        var srFilter  = curriedFilter(e => e.Age > 30);

        var itEmps   = itFilter(employees);
        var seniorEmps = srFilter(employees);

        PrintHeader(_output, "IT Department");
        foreach (var e in itEmps)
            _output.WriteLine(e.Name);

        PrintHeader(_output, "Age > 30");
        foreach (var e in seniorEmps)
            _output.WriteLine(e.Name);
        
        return unit;
    }

    private static IEnumerable<Employee> SampleEmployees() =>
    [
        new Employee("Johnny",29, "IT"),
        new Employee("Fred",32, "HR"),
        new Employee("Judy",45, "IT"),
        new Employee("Suzie",34, "HR")
    ];
}
