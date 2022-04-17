using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerStorage))]
public class PlayerStorageEditor : Editor
{
    private bool _putRandomNumbers;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var playerStorage = (PlayerStorage) target;

        GUILayout.Space(10f);

        if (GUILayout.Button("Generate Money Data"))
        {
            PlayerStorage.MoneyData.PutRandomNumbers();
        }

        GUILayout.Space(20f);

        if (GUILayout.Button("Distinct"))
        {
            playerStorage.Distinct();
        }
        GUILayout.Space(10f);

        if (GUILayout.Button("Load Objects From Database"))
        {
            playerStorage.LoadFromDatabase(_putRandomNumbers);
        }
        _putRandomNumbers = GUILayout.Toggle(_putRandomNumbers, new GUIContent("Put Random Numbers"));
    }
}
