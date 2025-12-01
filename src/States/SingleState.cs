using src.Entity;
using src.Interface;

namespace src.States;

public class SingleState : ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        //在quote 模式的时候碰见单引号则结束
        if (c == '\'')
        {
            context.state = new NormalState();
            return;
        }

        context.Append(c);
    }
}