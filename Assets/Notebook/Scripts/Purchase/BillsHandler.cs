using System.Text;
using TMPro;
using UnityEngine;

public class BillsHandler : MonoBehaviour
{
    [Header("Earned")] 
    [SerializeField] private TMP_Text _stockText;
    [SerializeField] private TMP_Text _earnedText;
    [SerializeField] private TMP_Text _ordersText;
    [SerializeField] private TMP_Text _sumText;

    [Header("Purchase")]
    [SerializeField] private TMP_Text _wasText;
    [SerializeField] private TMP_Text _purchaseText;
    [SerializeField] private TMP_Text _totalText;

    public void SetEarnedText()
    {
        _stockText.text = $"{PlayerStorage.MoneyData.Stock}$";
        _earnedText.text = $"{PlayerStorage.MoneyData.GainedOnCurrentDay}$";
        _sumText.text = $"{PlayerStorage.MoneyData.TotalMoney}$";
    }

    public void SetPurchaseBillText()
    {
        _wasText.text = $"{PlayerStorage.MoneyData.TotalMoney}$";
        _totalText.text = $"{PlayerStorage.MoneyData.CurrentMoney}$";
        var stringBuilder = new StringBuilder();
        PlayerStorage.MoneyData.CurrentBill.ForEachAction(data =>
        {
            if(data.cost == 0) return;
            stringBuilder.AppendLine($"{data.name}\t<color=yellow>{data.cost}$</color>\n");
        });
        _purchaseText.richText = true;
        _purchaseText.text = stringBuilder.ToString();
    }
}
