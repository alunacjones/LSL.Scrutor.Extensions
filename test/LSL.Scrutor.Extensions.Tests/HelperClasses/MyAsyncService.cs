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

public interface IRemove { }

public interface IGeneric<T> : IRemove
{
    T MyTest();
}

public class Generic<T> : IGeneric<T>
{
    public T MyTest() => default;
}

public class GenericInt : IGenericInt
{
    public int MyTest() => default!;
}

public interface IGenericInt : IGeneric<int>
{

}