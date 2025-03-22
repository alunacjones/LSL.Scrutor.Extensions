namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class MyService : IMyService
{
    private readonly AnotherDependency _other;
    private readonly string _name;
    
    public MyService(string name, AnotherDependency other)
    {
        _name = name;
        _other = other;
    }

    public string Name => _other.FormatName(_name);
}
