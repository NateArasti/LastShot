using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterDatabase))]
class CharacterDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var database = (CharacterDatabase)target;
        GUILayout.Space(20);

        if (GUILayout.Button("Load Name Table Data")) database.LoadNameTableData();
        GUILayout.Space(40f);
        if (GUILayout.Button("Fill Dictionaries")) database.FillDictionaries();
        if (GUILayout.Button("Clear Dictionaries")) database.ClearDictionaries();
    }
}