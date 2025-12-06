using src.Entity;
using src.Interface;

namespace src.States;

/// <summary>
/// 双引号模式，将内容全部识别为一个 token
/// </summary>
public class DoubleQuotesState : ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        if (c == '\"')
        {
            context.SetState(new NormalState());
            return;
        }

        if (c == '\\')
        {
            context.SetState(new BackslashState());
            return;
        }

        context.Append(c);
    }
}