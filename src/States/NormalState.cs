using src.Entity;
using src.Interface;

namespace src.States;

/// <summary>
///  常规模式，将按照输入的空格来区分 token，如果识别到其他模式的触发字符则转向其他模式
/// </summary>
public class NormalState : ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        if (c == '\\')
        {
            context.SetState(new BackslashState());
            return;
        }
        //如果识别到单引号则切换为 quote 模式 
        if (c == '\'')
        {
            context.SetState(new SingleState());
            return;
        }

        if (c == '\"')
        {
            context.SetState(new DoubleQuotesState());
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