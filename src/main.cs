class Program
{
    static void Main()
    {
        // TODO: Uncomment the code below to pass the first stage
        Console.Write("$ ");
        var command = Console.ReadLine();
        var commandList=new List<string>();
        if(!commandList.Contains(command))
        {
            Console.WriteLine($"{command}: command not found");
        }
    }
}
