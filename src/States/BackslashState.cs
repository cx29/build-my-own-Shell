using src.Entity;
using src.Interface;

namespace src.States;

public class BackslashState:ITokenizerState
{
    public void HandleToken(Tokenizer context, char c)
    {
        context.Append(c);
        context.state = new NormalState();
    }
}