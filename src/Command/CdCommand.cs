using src.Entity;
using src.Interface;

namespace src.Command;

public class CdCommand : IMyCommand
{
    private const string currDir = "./";
    private const string prevDir = "../";
    private const string HOMEDir = "~";
    private string HOME => Environment.GetEnvironmentVariable("HOME");
    public string Name => "cd";
    public string Description => "to target directory";

    public Task<int> ExecuteAsync(string[] args, ShellContext context, Func<string, string?> func)
    {
        if (args.Length < 1) return Task.FromResult(0);
        if (ResolvePath(args[0], context) is { } path)
        {
            context.CurrentDir = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
        else
        {
            Console.WriteLine($"cd: {args[0]}: No such file or directory");
        }

        return Task.FromResult(0);
    }

    private string? ResolvePath(string path, ShellContext context)
    {
        if (path.Equals(HOMEDir))
            return HOME;
        if (!path.StartsWith(currDir) && !path.StartsWith(prevDir)) return Directory.Exists(path) ? path : null;
        //获取完整路径，直接判断是否存在
        var fullPath = Path.GetFullPath(path, context.CurrentDir);
        return Directory.Exists(fullPath) ? fullPath : null;
    }
}