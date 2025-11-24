using src.Entity;
using src.Interface;

namespace src.Command;

public class CdCommand : IMyCommand
{
    public string Name => "cd";
    public string Description => "to target directory";

    public Task<int> ExecuteAsync(string[] args, ShellContext context, Func<string, string?> func)
    {
        if (args.Length >= 1)
        {
            if (Directory.Exists(args[0]))
            {
                context.CurrentDir=args[0];
            }
            else
            {
                Console.WriteLine($"cd: {args[0]}: No such file or directory");
            }
        }
        return Task.FromResult(0);
    }
}