using System.Threading.Tasks;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class MyAsyncService : IMyAsyncService
{
    private readonly IConsole _console;

    public MyAsyncService(IConsole console)
    {
        _console = console;
    }

    public async Task RunAsync()
    {
        await Task.Delay(1000);
        _console.WriteLine("My output");
    }
}
