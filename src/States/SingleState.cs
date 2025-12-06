using src.Entity;
using src.Interface;

namespace src.States;

/// <summary>
///  单引号模式， 将引号内的所有字符识别为一个 token
/// </summary>
public class SingleState : ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        //在quote 模式的时候碰见单引号则结束
        if (c == '\'')
        {
            context.SetState(new NormalState());
            return;
        }

        context.Append(c);
    }
}