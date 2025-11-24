using src.Entity;
using src.Interface;

namespace src.Command;

public class TypeCommand : IMyCommand
{
    public string Name => "type";
    public string Description => "Type Command";

    public Task<int> ExecuteAsync(string[] args, ShellContext context, Func<string, string?> func)
    {
        foreach (var cmd in args)
        {
            var line = context.BuildInList.Contains(cmd) ? $"{cmd} is a shell builtin" : func(cmd) is { } res ? $"{cmd} is {res}" : $"{cmd}: not found";
            Console.WriteLine(line);
        }

        return Task.FromResult(0);
    }
}