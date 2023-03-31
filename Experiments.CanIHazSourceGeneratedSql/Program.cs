namespace Experiments.CanIHazSourceGeneratedSql;

public class Program
{
    public static void Main(string[] args)
    {
        // example of direct usage
        var testQuery = SqlFiles.TestQuery;
        var testAnotherQuery = SqlFiles.Repositories.TestAnotherQuery;
    }
}
