using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkInfoPage : MonoBehaviour
{
    [SerializeField] private bool _showFullInfo;
    [Header("Data Handlers")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _tags;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _receipt;
    [SerializeField] private Image _icon;

    private FastAction _onClick = new FastAction();

    public void SetInfo(Drink drink, System.Action<Drink> onChoose)
    {
        if(onChoose != null)
            _onClick.Add(() => onChoose.Invoke(drink));

        var (name, tags, description, textReceipt, icon) = drink.InfoData;
        _icon.sprite = icon;
        _name.text = name;
        _tags.text = string.Join("/ ", tags);
        if (_showFullInfo)
        {
            _description.text = description;
            _receipt.text = textReceipt;
        }
    }

    public void SetInfo(Drink drink) => SetInfo(drink, null);

    public void OnClick() => _onClick.Call();
}
