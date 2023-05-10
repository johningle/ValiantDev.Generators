namespace ValiantDev.Generators.Tests.Unit;

public class JsonFilesTests
{
    [Fact]
    public void Contents_of_JSON_files_in_root_match_JsonFiles_string()
    {
        var expected = @"{
  ""key"": ""value""
}";
        Assert.Equal(expected, JsonFiles.Root);
    }
}
