using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class DialogueDrinkPanel : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _tags;
    [SerializeField] private TMP_Text _description;
    private Button _button;

    private void Awake() => _button = GetComponent<Button>();

    public void SetData(Drink drink, UnityEvent<Drink> chooseEvent)
    {
        var infoData = drink.InfoData;
        if (infoData.Locked) Destroy(gameObject);
        _icon.sprite = infoData.ObjectSprite;
        _name.text = infoData.Name;
        _tags.text = string.Join(" / ", infoData.Tags.Select(drinkTag => drinkTag.ToString()));
        _description.text = infoData.Description;

        _button.onClick.AddListener(() => chooseEvent.Invoke(drink));
    }
}
