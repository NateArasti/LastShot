using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IngredientDatabase))]
class IngredientDatabaseEditor : Editor
{
    private SerializedProperty _spritesFolderPath;

    private void OnEnable()
    {
        _spritesFolderPath = serializedObject.FindProperty("_spritesFolderPath");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var database = (IngredientDatabase)target;
        GUILayout.Space(20);

        if (GUILayout.Button("Load Objects From Resources")) database.LoadFromResources();
        GUILayout.Space(5);

        if (GUILayout.Button("Load Table Data")) database.LoadTableData();
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(_spritesFolderPath);
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);

        if (GUILayout.Button("Load Sprites From Resources")) database.LoadSpritesFromResources();

        if (GUILayout.Button("Distinct")) database.Distinct();
    }
}