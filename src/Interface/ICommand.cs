using src.Entity;

namespace src.Interface;

public interface IMyCommand
{
    string Name { get; }
    string Description { get; }

    /// <summary>
    /// 执行逻辑
    /// </summary>
    /// <param name="args"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    Task<int> ExecuteAsync(List<string> args, ShellContext context, Func<string,string?> func);
}