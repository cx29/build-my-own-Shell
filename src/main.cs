using System.Runtime.InteropServices;

class Program
{
    static readonly HashSet<string> validCommandList = new()
    {
        "echo", "exit", "type"
    };

    static void Main()
    {
        while (true)
        {
            Console.Write("$ ");
            //手动刷新缓冲区
            Console.Out.Flush();
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                continue;
            var parts = ParseCommand(input);
            //区分为指令和参数两部分
            var cmd = parts.cmd; //取出指令部分
            //取出参数部分
            var args = parts.args;
            var res = ExecuteCommand(cmd, args);
            if (!res)
            {
                Console.WriteLine($"{cmd}: command not found");
            }
        }
    }

    /// <summary>
    /// 解析 cmd
    /// </summary>
    /// <param name="input">用户输入</param>
    /// <returns>(指令，参数)</returns>
    static (string cmd, string args) ParseCommand(string input)
    {
        // 去除分割字符之后出现的空内容项
        var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => ("", ""),
            1 => (parts[0], ""),
            _ => (parts[0], parts[1])
        };
    }

    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    static bool ExecuteCommand(string cmd, string args)
    {
        switch (cmd)
        {
            case "exit":
                if (args == "0") Environment.Exit(0);
                return true;
            case "echo":
                Console.WriteLine(args);
                return true;
            case "type":
            {
                // 解析多个参数
                var argList = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var arg in argList)
                {
                    Console.WriteLine(FindExecutableInPath(arg) is { } res ? $"{arg} is {res}" : $"{arg}: not found");
                }

                return true;
            }
            default:
                return false;
        }
    }

    /// <summary>
    /// 在环境变量中查找指令是否可执行
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    static string? FindExecutableInPath(string arg)
    {
        // 获取环境变量下的路径字符串
        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrWhiteSpace(pathEnv))
        {
            return null;
        }

        // 根据分割符进行环境变量的分割
        var paths = pathEnv.Split(Path.PathSeparator);
        foreach (var path in paths)
        {
            if (CanExecuteFile(path, arg) is { } fullPath)
            {
                return fullPath;
            }
        }

        return null;
    }


    /// <summary>
    /// 判定文件是否为可执行文件，区分平台
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cmd"></param>
    /// <returns></returns>
    static string? CanExecuteFile(string path, string cmd)
    {
        // 如果是 windows 平台的话则根据后缀来进行判定可执行文件
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            //windows 中可执行文件的后缀
            var extensions = Environment.GetEnvironmentVariable("PATHEXT")?.Split(';') ?? [".exe", ".bat", ".cmd", ".com"];
            return extensions.Select(ext => Path.Combine(path, cmd + ext)).FirstOrDefault(File.Exists);
        }
        else //判定 unix
        {
            var fullPath = Path.Combine(path, cmd);
            //判断文件是否存在， 判断文件是否有可执行权限
            if (File.Exists(fullPath) && access(fullPath, X_OK) == 0)
            {
                return fullPath;
            }
        }

        return null;
    }

    [DllImport("libc", SetLastError = true)]
    private static extern int access(string path, int mode);

    // 检查可执行权限
    private const int X_OK = 1;
}