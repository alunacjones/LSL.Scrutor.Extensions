using LSL.AbstractConsole;

namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class SyncServiceToDecorate(IConsole console) : ISyncServiceToDecorate
{
    public void DoSomething() => console.WriteLine("Something done");
}