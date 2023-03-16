using System.Reflection;
using Generators;

namespace Experiments.CanIHazSourceGeneratedSql;

public class Program
{
    public static void Main(string[] args)
    {
        foreach (var member in typeof(SqlFiles).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            var name = member.Name;
            var value = member.GetValue(null)!.ToString();
            Console.WriteLine($"{Environment.NewLine}{name}: {value}");
        }

        // example of direct usage
        var someSql = SqlFiles.TestQuery;

        /*
        // debug generator
        var generator = new CodeGenerator();
        generator.AddSqlMember("foo", "bar");
        Console.WriteLine(generator.GetSourceCode());
        */
    }
}
