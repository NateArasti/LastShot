using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RandomIngredientIcon : MonoBehaviour
{
    private Image _image;
    private List<Sprite> _ingredientIcons;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _ingredientIcons = DatabaseManager.AlcoholDatabase.GetObjectsCollection()
            .Concat(DatabaseManager.AdditionalIngredientDatabase.GetObjectsCollection())
            .Select(alcohol => alcohol.Data.ObjectSprite).ToList();
        SetRandomIcon();
    }

    public void SetRandomIcon()
    {
        _image.sprite = _ingredientIcons.GetRandomObject();
    }
}
