using src.Entity;
using src.Interface;

namespace src.States;

public class NormalState : ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        //如果识别到单引号则切换为 quote 模式 
        if (c == '\'')
        {
            context.state = new SingleState();
            return;
        }

        //如果碰到空格则结束当前 token 的记录
        if (char.IsWhiteSpace(c))
        {
            context.EmitToken();
            return;
        }

        //普通字符则继续添加 token 内容
        context.Append(c);
    }
}