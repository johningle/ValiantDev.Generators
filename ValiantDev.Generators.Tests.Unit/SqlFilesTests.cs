namespace ValiantDev.Generators.Tests.Unit;

public class SqlFilesTests
{
    [Fact]
    public void Contents_of_SQL_files_in_root_match_SqlFiles_string()
    {
        var expected = @"SELECT *
FROM SomeTable
WHERE AColumn = 'a value'";
        Assert.Equal(expected, SqlFiles.Root);
    }

    [Fact]
    public void Contents_of_SQL_files_in_subdirectory_match_SqlFiles_nested_string()
    {
        var expected = @"SELECT id, name, description
FROM AnotherTable at
WHERE name LIKE 'Prefixed%'
";
        Assert.Equal(expected, SqlFiles.Subdirectory.Nested);
    }

    [Fact]
    public void Contents_of_SQL_files_in_more_deeply_nested_subdirectory_match_SqlFiles_deeply_nested_string()
    {
        var expected = @"-- Let's qualify this with a comment to explain our reasoning for nesting this file so deep.
-- Also, let's try some more SQL features, such as a CTE declaration.
;WITH SomeCTE AS (
    SELECT id, name
    FROM EnumTable
)
SELECT *
FROM OtherTable ot
JOIN SomeCTE sc on ot.SomeId = sc.id";
        Assert.Equal(expected, SqlFiles.Subdirectory.Deep.Deeper.DeeperStill.Fathoms);
    }
}