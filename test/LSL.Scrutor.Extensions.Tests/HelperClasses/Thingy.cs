using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests;

public class Thingy : IThingy
{
    private readonly IConsole _console;

    public Thingy(IConsole console) => _console = console;

    public void DoSomething() => _console.WriteLine("Something done");
}