using LSL.Scrutor.Extensions.Tests.HelperClasses;

namespace LSL.Scrutor.Extensions.Tests;

public interface IMyFactory
{
    IMyService Create(string name);
}

public interface IMyOtherFactory
{
    MyService Create(string name);
}
