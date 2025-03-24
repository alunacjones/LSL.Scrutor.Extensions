namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class MyService(string name, Prefixer prefixer) : IMyService
{
    public string Name => prefixer.FormatName(name);
}
