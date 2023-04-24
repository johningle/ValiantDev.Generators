namespace Experiments.CanIHazSourceGeneratedSql;

public static class Program
{
    public static void Main(string[] args)
    {
        // example of direct usage
        Console.WriteLine(SqlFiles.TestQuery);
        Console.WriteLine(SqlFiles.Repositories.TestAnotherQuery);
        Console.WriteLine(SqlFiles.Repositories.Queries.TestRead);
        Console.WriteLine(SqlFiles.Repositories.Commands.Update);
    }
}