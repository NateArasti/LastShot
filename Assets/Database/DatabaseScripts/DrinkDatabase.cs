using UnityEngine;

[CreateAssetMenu(fileName = "DrinkDatabase", menuName = "Data/Database/DrinkDatabase")]
public class DrinkDatabase : Database<Drink>
{
    [SerializeField] [HideInInspector]
    private string _spritesFolderPath;

    public void LoadSpritesFromResources()
    {
        foreach (var sprite in Resources.LoadAll<Sprite>(_spritesFolderPath))
        {
            if (TryGetValue(sprite.name, out var drink))
                drink.Sprite = sprite;
        }
        Debug.Log($"Loaded Sprites for {name} successfully");
    }
}