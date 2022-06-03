using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradesShowList : MonoBehaviour
{
    [SerializeField] private TMP_Text _header;
    [SerializeField] private UpgradeShowElement _upgradeShowElement;
    [SerializeField] private ScrollContentFiller _scrollContentFiller;
    private UpgradeShowPanel.ChooseType _chooseType;
    private IReadOnlyDictionary<UpgradeShowPanel.UpgradeShowData, UpgradeShowElement> _showElements;

    public void SetListData(
        UpgradeShowPanel.ChooseType chooseType,
        string header,
        UpgradeShowPanel.UpgradeShowData[] upgrades)
    {
        _chooseType = chooseType;
        _header.text = header;
        _showElements = _scrollContentFiller.FillContent(
            _upgradeShowElement,
            upgrades,
            (element, data) => element.SetData(data, UpdateState),
            true
        );

        _showElements.ForEachAction(upgrade =>
        {
            upgrade.Value.SetIcon(upgrade.Key.Shown);
            upgrade.Key.Toggle();
        });
    }

    private void UpdateState(UpgradeShowPanel.UpgradeShowData data)
    {
        if(_chooseType == UpgradeShowPanel.ChooseType.Multiple)
            data.Shown = !data.Shown;
        else if (!data.Shown)
            data.Shown = true;
        _showElements.ForEachAction(upgrade =>
        {
            if (_chooseType == UpgradeShowPanel.ChooseType.Select && upgrade.Key.Name != data.Name)
            {
                upgrade.Key.Shown = false;
            }
            upgrade.Value.SetIcon(upgrade.Key.Shown);
            upgrade.Key.Toggle();
        });
    }
}
