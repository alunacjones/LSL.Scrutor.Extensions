namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class MyService : IMyService
{
    private readonly Prefixer _other;
    private readonly string _name;
    
    public MyService(string name, Prefixer other)
    {
        _name = name;
        _other = other;
    }

    public string Name => _other.FormatName(_name);
}
