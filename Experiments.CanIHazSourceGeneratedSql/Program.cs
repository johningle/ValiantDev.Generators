using System.Reflection;

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
    }
}
