using src.Entity;
using src.Interface;

namespace src.Command;

public class EchoCommand : IMyCommand
{
    public string Name => "echo";
    public string Description => "Echo a command";

    /// <summary>
    /// 异步执行指令 
    /// </summary>
    /// <param name="args"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<int> ExecuteAsync(List<string> args, ShellContext context, Func<string, string?> func)
    {
        Console.WriteLine(string.Join(' ', args));
        return Task.FromResult(0);
    }
}