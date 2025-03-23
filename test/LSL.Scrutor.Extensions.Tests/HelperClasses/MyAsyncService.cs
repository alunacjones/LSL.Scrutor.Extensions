using System.Threading.Tasks;
using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class MyAsyncService(IConsole console) : IMyAsyncService
{
    public async Task RunAsync()
    {
        await Task.Delay(1000);
        console.WriteLine("My output");
    }
}
