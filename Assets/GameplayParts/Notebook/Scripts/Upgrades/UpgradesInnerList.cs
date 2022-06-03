using TMPro;
using UnityEngine;

[RequireComponent(typeof(ScrollContentFiller))]
public class UpgradesInnerList : MonoBehaviour
{
    [SerializeField] private TMP_Text _header;
    [SerializeField] private TMP_Text _currentMoney;
    [SerializeField] private UpgradeElement _upgradeElementPrefab;
    private ScrollContentFiller _scrollContentFiller;
    private UpgradePanel.ChooseType _chooseType;
    private UpgradePanel.UpgradeData[] _upgrades;

    private void Awake()
    {
        _scrollContentFiller = GetComponent<ScrollContentFiller>();
    }

    public void SetListData(
        UpgradePanel.ChooseType chooseType, 
        string header, 
        UpgradePanel.UpgradeData[] upgrades)
    {
        _chooseType = chooseType;
        _header.text = header;
        _currentMoney.text = $"У вас: {PlayerStorage.MoneyData.TotalMoney}$";
        _upgrades = upgrades;
        SetList();
    }

    public void SetNewState(string keyName, UpgradePanel.UpgradeData.State newState)
    {
        if(newState != UpgradePanel.UpgradeData.State.Selected) return;
        _upgrades.ForEachAction(data =>
        {
            if (data.KeyName == keyName)
            {
                data.UpgradeState = newState;
                return;
            }

            if (_chooseType == UpgradePanel.ChooseType.Select &&
                data.UpgradeState == UpgradePanel.UpgradeData.State.Selected)
                data.UpgradeState = UpgradePanel.UpgradeData.State.Unselected;
        });
        SetList();
    }

    private void SetList()
    {
        _scrollContentFiller.FillContent(
            _upgradeElementPrefab,
            _upgrades,
            (element, data) => element.SetData(data, SetNewState),
            true
        );
    }
}
