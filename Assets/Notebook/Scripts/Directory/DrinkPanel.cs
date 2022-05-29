using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class DrinkPanel : MonoBehaviour
{
    private static int _spawnCount;
    private static DrinkPanel _currentChosen;

    [SerializeField] private GameObject _lock;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _tags;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Sprite _simplePanelSprite;
    [SerializeField] private Sprite _chosenPanelSprite;
    private Button _button;
    private Image _coverImage;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _coverImage = GetComponent<Image>();
    }

    public void SetData(Drink drink, UnityEvent<Drink> chooseEvent)
    {
        var infoData = drink.InfoData;
        _icon.sprite = infoData.ObjectSprite;
        _name.text = infoData.Name;
        _tags.text = string.Join(" / ", infoData.Tags.Select(drinkTag => drinkTag.ToString()));
        _description.text = infoData.Description;

        _button.onClick.AddListener(() =>
        {
            if(_currentChosen != null)
                _currentChosen._coverImage.sprite = _simplePanelSprite;
            _coverImage.sprite = _chosenPanelSprite;
            _currentChosen = this;
            chooseEvent.Invoke(drink);
        });
        _spawnCount += 1;
        if(_spawnCount == 1)
            _button.onClick.Invoke();

        _lock.SetActive(infoData.Locked);
        _button.interactable = !infoData.Locked;
    }

    public void ManuallyChoose() => _button.onClick.Invoke();
}
