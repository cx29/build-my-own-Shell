using src.Entity;
using src.Interface;

namespace src.Command;

public class ExitCommand : IMyCommand
{
    public string Name => "exit";
    public string Description => "exit";

    public Task<int> ExecuteAsync(List<string> args, ShellContext context, Func<string, string?> func)
    {
        if (args.Count== 0 || args[0] == "0")
        {
            context.ShouldExit = true;
        }

        return Task.FromResult(0);
    }
}