using src.Entity;
using src.Interface;

namespace src.Command;

public class PwdCommand : IMyCommand
{
    public string Name => "pwd";
    public string Description => "show current Dir";

    public Task<int> ExecuteAsync(List<string> args, ShellContext context, Func<string, string?> func)
    {
        Console.WriteLine(context.CurrentDir);
        return Task.FromResult(0);
    }
}