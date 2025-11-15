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
            if (!commandList.Contains(command))
            {
                Console.WriteLine($"{command}: command not found");
            }
        }

    }
}
