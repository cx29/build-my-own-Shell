using src.Entity;
using src.Interface;

namespace src.States;

public class DoubleQuotesState : ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        if (c == '\"')
        {
            context.state = new NormalState();
            return;
        }

        context.Append(c);
    }
}