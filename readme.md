# Usage

I want this to be as simple as possible to use successfully.
A developer should only need to to complete the following steps to use the eventual NuGet package:

1. Add a reference to the NuGet package from the project where they want to include and use the `.sql` files.
2. Add the following section to their .csproj file:
   ```
   <ItemGroup>
       <AdditionalFiles Include="**\*.sql" />
   </ItemGroup>
   ```
3. Add a file to their project ending in `.sql`.
4. Use the static class `SqlFiles` to access the contents of their `.sql` files as strings in their code: 
   ```
   var sqlString = SqlFiles.SomeQuery;
   ```

# SqlFiles Class

To avoid burdening users with further requirements than those listed in the Usage section, the following conventions are used.

## SqlFiles Namespace (Done)

The generated static `SqlFiles` class is placed in the project's root namespace.

## Mapping SQL File Path & Name to SqlFiles (Done)

To avoid restricting users' options for organizing their SQL files, both the file path and name are used to determine the const string location within `SqlFiles`.
Static inner classes are generated for each subdirectory in the file path, thereby providing a navigation chain reflecting the file system organization.

For example:

* Given a SQL file at: /Repositories/Widgets/Queries/GetWidgets.sql
* Access const string: SqlFiles.Repositories.Widgets.Queries.GetWidgets
