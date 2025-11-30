using src.Entity;
using src.Interface;

namespace src.Command;

public class CdCommand : IMyCommand
{
    private const string currDir = "./";
    private const string prevDir = "../";
    public string Name => "cd";
    public string Description => "to target directory";

    public Task<int> ExecuteAsync(string[] args, ShellContext context, Func<string, string?> func)
    {
        if (args.Length < 1) return Task.FromResult(0);
        if (ResolvePath(args[0], context) is { } path)
        {
            context.CurrentDir = path;
        }
        else
        {
            Console.WriteLine($"cd: {args[0]}: No such file or directory");
        }

        return Task.FromResult(0);
    }

    private string? ResolvePath(string path, ShellContext context)
    {
        //获取完整路径，直接判断是否存在
        var fullPath = Path.GetFullPath(path);
        return Directory.Exists(fullPath) ? fullPath : null;
    }
}