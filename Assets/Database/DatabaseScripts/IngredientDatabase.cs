using UnityEngine;

[CreateAssetMenu(fileName = "IngredientsDatabase", menuName = "Data/Database/IngredientDatabase")]
public class IngredientDatabase : Database<Ingredient>
{
    [SerializeField]
    private string _spritesFolderPath;

    public void LoadSpritesFromResources()
    {
        foreach (var sprite in Resources.LoadAll<Sprite>(_spritesFolderPath))
        {
            if (TryGetValue(sprite.name, out var ingredient))
                ingredient.Sprite = sprite;
        }
        Debug.Log($"Loaded Sprites for {name} successfully");
    }
}