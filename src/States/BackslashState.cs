using src.Entity;
using src.Interface;

namespace src.States;

/// <summary>
/// 区分上一个模式为什么，若为 DoubleQuotes 则需要判定当前字符串是否为指定的字符串
/// 将后面跟着的字符识正常加入
/// </summary>
public class BackslashState : ITokenizerState
{
    private readonly List<char> specialChars = ['\"', '\\', '\'', '$', 'n'];

    public void HandleToken(Tokenizer context, char c)
    {
        if (context.GetLastState is DoubleQuotesState doubleQuotesState)
        {
            if (!specialChars.Contains(c))
            {
                context.Append('\\');
            }
        }

        context.Append(c);
        context.SetState(context.GetLastState);
    }
}