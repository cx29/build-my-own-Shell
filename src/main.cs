class Program
{
    static void Main()
    {
        var validCommandList = new List<string>()
        {
            "echo", "exit"
        };
        while (true)
        {
            Console.Write("$ ");
            //手动刷新缓冲区
            Console.Out.Flush();
            var command = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(command))
                continue;
            //区分为指令和参数两部分
            var parts = command.Split(' ', 2);
            var cmd = parts[0]; //取出指令部分
            //取出参数部分j
            var args = parts.Length > 1 ? parts[1] : "";
            if (cmd == "exit" && args == "0")
                break;
            if (cmd == "echo")
            {
                Console.WriteLine(args);
            }
            else if (!validCommandList.Contains(cmd))
            {
                Console.WriteLine($"{cmd}: command not found");
            }
        }
    }
}