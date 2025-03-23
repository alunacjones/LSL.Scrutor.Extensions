using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class SyncServiceToDecorate : ISyncServiceToDecorate
{
    private readonly IConsole _console;

    public SyncServiceToDecorate(IConsole console) => _console = console;

    public void DoSomething() => _console.WriteLine("Something done");
}