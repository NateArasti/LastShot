using UnityEditor;
using UnityEngine;

public class SpriteDatabase<T> : Database<T> where T : Object, IDatabaseObject, ISpriteDatabaseObject
{
    [SerializeField] private string _spritesFolderPath;

    public void LoadSpritesFromResources()
    {
        foreach (var sprite in Resources.LoadAll<Sprite>(_spritesFolderPath))
        {
            if (TryGetValue(sprite.name, out var obj))
            {
                obj.Sprite = sprite;
#if UNITY_EDITOR
                EditorUtility.SetDirty(obj);
#endif
            }
            else
            {
                Debug.LogWarning($"No object with {sprite.name}");
            }
        }
        Debug.Log($"Loaded Sprites for {name} successfully");
    }
}
