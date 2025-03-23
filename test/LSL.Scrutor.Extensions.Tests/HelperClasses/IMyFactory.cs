namespace LSL.Scrutor.Extensions.Tests.HelperClasses;

public interface IMyFactory
{
    IMyService Create(string name);
}
