using System.Text;
using src.Interface;
using src.States;

namespace src.Entity;

public class Tokenizer
{
    public ITokenizerState state { get; set; } = new NormalState();
    public List<string> Tokens { get; } = new();
    private StringBuilder buffer = new();

    public void Append(char c) => buffer.Append(c);

    public void EmitToken()
    {
        if (buffer.Length <= 0) return;
        Tokens.Add(buffer.ToString());
        buffer.Clear();
    }

    public List<string> Tokenize(string input)
    {
        foreach (var c in input)
        {
            state.HandleToken(this, c);
        }

        EmitToken();
        return Tokens;
    }
}