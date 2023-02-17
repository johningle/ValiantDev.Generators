# Usage

I want this to be as simple as possible to use successfully.
A developer should only need to to complete the following steps to use the NuGet package:

1. Add reference to NuGet package from the project where they want to use the source generator.
2. Minimally edit the project file to ensure all files matching the glob *.sql are included.
3. Add a file to their project ending in .sql (should only contain SQL code).
4. Use the static class SqlFiles to access the contents of their SQL files as strings in their code (e.g. SqlFiles.SomeQuery).

# Requirements to Develop Incremental Source Generators

TBD