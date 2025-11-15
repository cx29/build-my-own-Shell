class Program
{
    static void Main()
    {
        var commandList = new List<string>()
        {

        };
        while (true)
        {
            Console.Write("$ ");
            var command = Console.ReadLine();
            if (command == "exit 0")
                break;
            else if (command.StartsWith("echo"))
            {
                Console.WriteLine(command.Replace("echo ", "") + Environment.NewLine);
            }
            else
                        if (!commandList.Contains(command))
            {
                Console.WriteLine($"{command}: command not found");
            }
        }

    }
}
