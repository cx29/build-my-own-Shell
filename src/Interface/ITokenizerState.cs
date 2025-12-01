using src.Entity;

namespace src.Interface;

public interface ITokenizerState
{
    void HandleToken(Tokenizer context,char c);
}