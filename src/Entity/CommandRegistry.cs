using src.Interface;

namespace src.Entity;

public class CommandRegistry
{
    /// <summary>
    /// 按二进制序号进行字符串比较，忽略大小写
    /// </summary>
    private readonly Dictionary<string, IMyCommand> _commands = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 注册单个指令
    /// </summary>
    /// <param name="command"></param>
    public void Register(IMyCommand command)
    {
        _commands[command.Name] = command;
    }

    /// <summary>
    /// 获取内置指令名字
    /// </summary>
    /// <returns></returns>
    public List<string> GetBuildInCmdName()
    {
        return _commands.Keys.ToList();
    }

    /// <summary>
    /// 通过名字查找指令
    /// </summary>
    /// <param name="name"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool TryGet(string name, out IMyCommand? command)
        => _commands.TryGetValue(name, out command);

    /// <summary>
    /// 获取所有指令
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IMyCommand> GetAll() => _commands.Values;
}