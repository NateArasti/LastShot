using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcurePanel : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _totalCost;
    [SerializeField] private TMP_InputField _amountField;

    private int _costPerObject;

    public void SetProcureData(Ingredient ingredient)
    {
        _costPerObject = ingredient.Data.costPerObject;
        _icon.sprite = ingredient.Sprite;
        _name.text = ingredient.Data.name;
    }

    public void ChangeAmount(int delta)
    {
        _amountField.text = (int.Parse(_amountField.text) + delta).ToString();
        ShowTotalCost();
    }

    public void ShowTotalCost()
    {
        if(_amountField.text == "")
        {
            _amountField.text = "0";
        }
        _totalCost.text = $"{_costPerObject * int.Parse(_amountField.text)}$";
    }
    
    public void SaveProcureAmount()
    {
        print("saving...");
    }
}
