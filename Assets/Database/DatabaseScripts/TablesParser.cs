using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class TablesParser
{
    public static void WriteParsedTableData<T>(this Database<T> database, TextAsset objectsTable) where T : Object, IDatabaseObject, ISpriteDatabaseObject
    {
        var parsedTable = GetParsedTable(objectsTable);
        for (var i = 1; i < parsedTable.Count; i++)
        {
            if (database.TryGetValue(parsedTable[i][0], out var databaseObject) && parsedTable[i].Length > 1)
            {
                databaseObject.WriteData(parsedTable[i]);
                EditorUtility.SetDirty(databaseObject);
            }
        }
    }

    public static List<string[]> GetParsedTable(TextAsset table) => 
            table.text
            .Split('\n')
            .Select(textLines => textLines.Split(';'))
            .ToList();

    public static Dictionary<string, float> GetParsedCoefficientsTable(TextAsset table) => 
        GetParsedTable(table)
            .Skip(1)
            .ToDictionary(line => line[0], line => float.Parse(line[2]));
}
