namespace Experiments.CanIHazSourceGeneratedSql;

public static class Program
{
    public static void Main(string[] args)
    {
        // example of direct usage
        var testQuery = SqlFiles.TestQuery;
        Console.WriteLine(testQuery);
        var testAnotherQuery = SqlFiles.Repositories.TestAnotherQuery;
        Console.WriteLine(testAnotherQuery);
    }
}