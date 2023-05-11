using System.Data.Common;

namespace ValiantDev.Generators;

/// <summary>
/// Extend the partial class created by the SqlFiles generator to explicitly include additional members.
/// This could be useful in cases where you want to include data or functionality associated to your usage of SqlFiles that is not actually a .sql file.
/// </summary>
public static partial class SqlFiles
{
    public static readonly string ConnectionString = @"Server=db.somewhere.example.com;Initial Catalog=the_db";
}
