using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AlcoholShelfSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _chosenAlcoholPrefab;
    [SerializeField] private RectTransform _chosenAlcoholsPivot;
    [SerializeField] private Button _finishAlcoholCollection;
    [SerializeField] private ShelfBottle[] _bottles;

    private Ingredient[] _currentReceiptAlcohols;
    private (Button button, Image iconImage, ShelfBottle shelfBottle)[] _chosenAlcohols;

    private void Start()
    {
        var bottlesList = _bottles.ToList();
        var alcohols = DatabaseManager.AlcoholDatabase.GetObjectsCollection();
        foreach (var alcohol in alcohols)
        {
            var bottleIndex = Random.Range(0, bottlesList.Count);
            bottlesList[bottleIndex].SetAlcohol(alcohol, 
                (chosenAlcohol, shelfBottle) => 
                    {
                        var result = _currentReceiptAlcohols.Contains(chosenAlcohol);
                        if(result)
                            ChooseAlcohol(chosenAlcohol, shelfBottle);
                        return result;
                    }
                );
            bottlesList.RemoveAt(bottleIndex);
        }
        _finishAlcoholCollection.transform.SetAsLastSibling();
    }

    public void SetReceipt(Drink drink)
    {
        _currentReceiptAlcohols = drink.Receipt.alcohols.ToArray();
        _chosenAlcohols = new (Button button, Image image, ShelfBottle shelfBottle)[_currentReceiptAlcohols.Length];
        for(var i = 0; i < _currentReceiptAlcohols.Length; ++i)
        {
            var spawnedAlcohol = Instantiate(_chosenAlcoholPrefab, _chosenAlcoholsPivot);
            _chosenAlcohols[i] = (spawnedAlcohol.GetComponent<Button>(), spawnedAlcohol.transform.GetChild(0).GetComponent<Image>(), null);
            _chosenAlcohols[i].iconImage.enabled = false;
            _chosenAlcohols[i].button.interactable = false;
        }
        _finishAlcoholCollection.interactable = false;
        _finishAlcoholCollection.onClick.AddListener(() => GamePartsSwitch.SwitchToWorkSpace(drink));
    }

    private void ChooseAlcohol(Ingredient alcohol, ShelfBottle shelfBottle)
    {
        for(var i = 0; i < _chosenAlcohols.Length; ++i)
        {
            if (_chosenAlcohols[i].shelfBottle = null) continue;
            _chosenAlcohols[i].shelfBottle = shelfBottle;
            _chosenAlcohols[i].iconImage.enabled = true;
            _chosenAlcohols[i].iconImage.sprite = alcohol.Sprite;
            _chosenAlcohols[i].button.interactable = true;
            var j = i;
            _chosenAlcohols[i].button.onClick.AddListener(() => Unchoose(j));

            _finishAlcoholCollection.interactable = i == _chosenAlcohols.Length - 1;
            break;
        }
    }

    private void Unchoose(int index)
    {
        _chosenAlcohols[index].shelfBottle.Unchoose();
        for (var i = index; i < _chosenAlcohols.Length - 1; ++i)
        {
            _chosenAlcohols[i].shelfBottle = _chosenAlcohols[i + 1].shelfBottle;
            _chosenAlcohols[i].iconImage.enabled = _chosenAlcohols[i + 1].iconImage.enabled;
            _chosenAlcohols[i].iconImage.sprite = _chosenAlcohols[i + 1].iconImage.sprite;
            _chosenAlcohols[i].button.interactable = _chosenAlcohols[i + 1].button.interactable;
            _chosenAlcohols[i].button.onClick = _chosenAlcohols[i + 1].button.onClick;
        }
        _chosenAlcohols[_chosenAlcohols.Length - 1].shelfBottle = null;
        _chosenAlcohols[_chosenAlcohols.Length - 1].iconImage.enabled = false;
        _chosenAlcohols[_chosenAlcohols.Length - 1].iconImage.sprite = null;
        _chosenAlcohols[_chosenAlcohols.Length - 1].button.interactable = false;
        _chosenAlcohols[_chosenAlcohols.Length - 1].button.onClick.RemoveAllListeners();
    }
}
