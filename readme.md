# Usage

I want this to be as simple as possible to use successfully.
A developer should only need to to complete the following steps to use the NuGet package:

1. Add reference to NuGet package from the project where they want to use the source generator.
2. Add the following section to your .csproj file:
   ```
   <ItemGroup>
       <AdditionalFiles Include="*.sql" />
   </ItemGroup>
   ```
3. Add a file to their project ending in .sql (should only contain SQL code).
4. Use the static class SqlFiles to access the contents of their SQL files as strings in their code: 
   ```
   var sqlString = SqlFiles.SomeQuery;
   ```

# Requirements to Develop Incremental Source Generators

TBD