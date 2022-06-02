using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterDatabase))]
class CharacterDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var database = (CharacterDatabase)target;
        GUILayout.Space(10);

        if (GUILayout.Button("Load Objects From Resources")) database.LoadFromResources();

        GUILayout.Space(5);

        if (GUILayout.Button("Load Table Data")) database.LoadTableData();

        GUILayout.Space(5);

        if (GUILayout.Button("Distinct")) database.Distinct();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Character Names", EditorStyles.boldLabel);
        if (GUILayout.Button("Load Name Table Data")) database.LoadNameTableData();
        GUILayout.Space(10f);
        if (GUILayout.Button("Fill Dictionaries")) database.FillDictionaries();
        if (GUILayout.Button("Clear Dictionaries")) database.ClearDictionaries();
    }
}