-- Let's qualify this with a comment to explain our reasoning for nesting this file so deep.
-- Also, let's try some more SQL features, such as a CTE declaration.
;WITH SomeCTE AS (
    SELECT id, name
    FROM EnumTable
)
SELECT *
FROM OtherTable ot
JOIN SomeCTE sc on ot.SomeId = sc.id