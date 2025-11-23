namespace src.Entity;

/// <summary>
/// 指令上下文
/// </summary>
public class ShellContext
{
    ///当前路径
    public string CurrentDir { get; set; } = Directory.GetCurrentDirectory();

    public List<string> BuildInList { get; } = new();

    ///环境变量
    public Dictionary<string, string> EnvironmentVariables { get; } = new();

    /// 标准输入输出
    public TextWriter Output { get; set; } = Console.Out;

    /// <summary>
    /// 上一条指令退出码
    /// </summary>
    public TextWriter Error { get; set; } = Console.Error;

    /// <summary>
    /// 是否退出 shell
    /// </summary>
    public bool ShouldExit { get; set; } = false;


    public ShellContext()
    {
        //初始化环境变量
        EnvironmentVariables["PATH"] = Environment.GetEnvironmentVariable("PATH") ?? "";
    }
}