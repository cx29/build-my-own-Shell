using src;

static class Program
{
    static async Task Main()
    {
        var shell = new Shell();
        await shell.RunAsync();
    }
}