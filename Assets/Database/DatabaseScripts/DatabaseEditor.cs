using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IngredientDatabase))]
class IngredientDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var database = (IngredientDatabase) target;
        GUILayout.Space(20);

        if (GUILayout.Button("Load Objects From Resources")) database.LoadFromResources();

        GUILayout.Space(10);

        if (GUILayout.Button("Load Table Data")) database.LoadTableData();

        GUILayout.Space(10);

        if (GUILayout.Button("Load Sprites From Resources")) database.LoadSpritesFromResources();

        GUILayout.Space(10);

        if (GUILayout.Button("Distinct")) database.Distinct();
    }
}

[CustomEditor(typeof(DrinkDatabase))]
class DrinkDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var database = (DrinkDatabase)target;
        GUILayout.Space(20);

        if (GUILayout.Button("Load Objects From Resources")) database.LoadFromResources();

        GUILayout.Space(10);

        if (GUILayout.Button("Load Table Data")) database.LoadTableData();

        GUILayout.Space(10);

        if (GUILayout.Button("Load Sprites From Resources")) database.LoadSpritesFromResources();

        GUILayout.Space(10);

        if (GUILayout.Button("Distinct")) database.Distinct();
    }
}