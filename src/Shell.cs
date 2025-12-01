using System.Diagnostics;
using System.Runtime.InteropServices;
using src.Command;
using src.Entity;

namespace src;

public class Shell
{
    //POSIX用法判断是否有可执行权限
    [DllImport("libc", SetLastError = true)]
    private static extern int access(string path, int mode);

    private const int X_OK = 1;

    private readonly ShellContext _context = new();
    private readonly CommandRegistry _registry = new();

    public Shell()
    {
        RegisterBuiltins();
    }

    /// <summary>
    ///  注册内建指令
    /// </summary>
    private void RegisterBuiltins()
    {
        _registry.Register(new EchoCommand());
        _registry.Register(new ExitCommand());
        _registry.Register(new TypeCommand());
        _registry.Register(new PwdCommand());
        _registry.Register(new CdCommand());

        //获取所有内置指令名字
        _context.BuildInList.AddRange(_registry.GetBuildInCmdName());
    }

    /// <summary>
    ///  主流程
    /// </summary>
    public async Task RunAsync()
    {
        while (!_context.ShouldExit)
        {
            Console.Write("$ ");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line)) continue;
            await ExecuteLineAsync(line);
        }
    }

    /// <summary>
    /// 指令执行 
    /// </summary>
    /// <param name="line"></param>
    private async Task ExecuteLineAsync(string line)
    {
        // 解析指令
        var parts = line.Split(' ', 2);
        var cmdName = parts[0];
        Tokenizer tokenizer = new();
        var args = tokenizer.Tokenize(parts.Skip(1).LastOrDefault());

        // 判断是否为内建指令
        if (_registry.TryGet(cmdName, out var command))
        {
            await command.ExecuteAsync(args, _context, ResolveCommandPath);
            return;
        }

        await ExecuteExternalAsync(cmdName, args);
    }

    /// <summary>
    /// 执行外部指令
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    private async Task ExecuteExternalAsync(string cmd, List<string> args)
    {
        // 查找实际可执行指令
        var realCmd = ResolveCommandPath(cmd);

        if (realCmd == null)
        {
            Console.WriteLine($"{cmd}: command not found");
            return;
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = cmd,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                UseShellExecute = false,
            };
            foreach (var arg in args)
            {
                startInfo.ArgumentList.Add(arg);
            }

            var process = Process.Start(startInfo);
            await process.WaitForExitAsync();
            _context.EnvironmentVariables["_"] = cmd;
        }
        catch (Exception e)
        {
            Console.WriteLine($"{cmd}: command not found");
        }
    }

    /// <summary>
    /// 判断指令的路径
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    private string? ResolveCommandPath(string cmd)
    {
        // 判断是否包含路径分隔符， 来区分是路径的指令还是加入环境变量或内建指令
        if (cmd.Contains(Path.DirectorySeparatorChar) || cmd.Contains(Path.AltDirectorySeparatorChar))
        {
            //是路径指令则需要判断文件是否存在
            if (!File.Exists(cmd)) return null;
            if (IsWindows() || IsUnix() && IsExecutableUnix(cmd))
                return cmd;
        }

        string[] winExts = new[] { ".exe", ".cmd", ".bat" };

        // 3. 在当前目录下查找是否有可执行文件
        var local = Path.Combine(Directory.GetCurrentDirectory(), cmd);
        if (File.Exists(local))
        {
            return local;
        }

        if (IsWindows())
        {
            foreach (var ext in winExts)
            {
                var localExt = local + ext;
                if (File.Exists(localExt))
                    return localExt;
            }
        }

        // 4. 获取环境变量，若环境变量为 0 则直接返回
        if (!_context.EnvironmentVariables.TryGetValue("PATH", out var pathVar))
            return null;

        //按照分隔符进行分隔
        var paths = pathVar.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

        foreach (var dir in paths)
        {
            if (!Directory.Exists(dir))
                continue;

            if (IsWindows())
            {
                foreach (var ext in winExts)
                {
                    var candidate = Path.Combine(dir, cmd + ext);
                    if (File.Exists(candidate))
                        return candidate;
                }
            }

            if (IsUnix())
            {
                var fullPath = Path.Combine(dir, cmd);
                if (File.Exists(fullPath) && IsExecutableUnix(fullPath))
                    return fullPath;
            }
        }

        return null;
    }

    /// <summary>
    /// 判断是否是 windows 系统
    /// </summary>
    /// <returns></returns>
    private bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// 判断是否为类 unix 系统
    /// </summary>
    /// <returns></returns>
    private bool IsUnix() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    ///  判断是否有可执行权限
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private bool IsExecutableUnix(string path)
    {
        var fileInfo = new FileInfo(path);

        return fileInfo.Exists && access(path, X_OK) == 0;
    }
}