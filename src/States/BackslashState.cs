using src.Entity;
using src.Interface;

namespace src.States;

/// <summary>
/// 将后面跟着的字符识正常加入
/// </summary>
public class BackslashState : ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        context.Append(c);
        context.SetState(context.GetLastState);
    }
}