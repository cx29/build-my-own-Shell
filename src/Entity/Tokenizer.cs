using System.Text;
using src.Interface;
using src.States;

namespace src.Entity;

/// <summary>
/// 记录token 与状态相关
/// </summary>
public class Tokenizer
{
    private ITokenizerState state { get; set; } = new NormalState();
    private List<string> Tokens { get; } = new();
    private StringBuilder buffer = new();
    private ITokenizerState lastState { get; set; }

    public void Append(char c) => buffer.Append(c);

    /// <summary>
    /// 结束一个 token 的识别 
    /// </summary>
    public void EmitToken()
    {
        if (buffer.Length <= 0) return;
        Tokens.Add(buffer.ToString());
        buffer.Clear();
    }

    /// <summary>
    /// 处理指令参数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public List<string> Tokenize(string? input)
    {
        if (input == null) return Tokens;
        foreach (var c in input)
        {
            state.HandleToken(this, c);
        }

        EmitToken();
        return Tokens;
    }

    /// <summary>
    /// 获取上一个状态方便状态之间的联动 
    /// </summary>
    public ITokenizerState GetLastState => lastState;

    /// <summary>
    /// 设置当前状态并且记录上一个状态是什么
    /// </summary>
    /// <param name="state"></param>
    public void SetState(ITokenizerState state)
    {
        lastState = this.state;
        this.state = state;
    }
}