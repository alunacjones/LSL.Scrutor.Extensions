namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public class MyService : IMyService
{
    private readonly Prefixer _prefixer;
    private readonly string _name;
    
    public MyService(string name, Prefixer prefixer)
    {
        _name = name;
        _prefixer = prefixer;
    }

    public string Name => _prefixer.FormatName(_name);
}
