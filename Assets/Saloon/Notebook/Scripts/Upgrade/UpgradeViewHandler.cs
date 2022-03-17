using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UpgradeViewHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _upgradesDropdown;
    [SerializeField] private Toggle _showToggle;
    [SerializeField] private TMP_Text _upgradeName;
    [SerializeField] private UnityEvent<ColorUpgrade> _onColorUpgradeChoose;
    [SerializeField] private UnityEvent _onSimpleUpgradeChoose;
    [SerializeField] private UnityEvent _onLightUpgradeChoose;
    private UpgradePart[] _upgradeParts;
    private int _currentPartIndex;

    private void Start()
    {
        _upgradeParts = GetComponentsInChildren<UpgradePart>();
        _upgradesDropdown.AddOptions(_upgradeParts.Select(upgrade => upgrade.name).ToList());
        _upgradesDropdown.onValueChanged.AddListener(index =>
        {
            _upgradeParts[_currentPartIndex].ReturnToStart();
            _currentPartIndex = index;
            _upgradeParts[_currentPartIndex].ReturnToStart();
            SetUpgradeData();
        });
        _upgradesDropdown.onValueChanged.Invoke(0);
        _showToggle.onValueChanged.AddListener(OnToggleChange);
    }

    public void SwitchToNext()
    {
        _upgradeParts[_currentPartIndex].SwitchToNext();
        SetUpgradeData();
    }

    public void SwitchToPrevious()
    {
        _upgradeParts[_currentPartIndex].SwitchToPrevious();
        SetUpgradeData();
    }

    private void OnToggleChange(bool isOn)
    {
        _upgradeParts[_currentPartIndex].ShowUpgrade(isOn);
    }

    public void Consolidate()
    {
        _upgradeParts[_currentPartIndex].ConsolidateUpgrade();
    }

    private void SetUpgradeData()
    {
        var data = _upgradeParts[_currentPartIndex].GetCurrentUpgradeData();
        _showToggle.isOn = data.isOn;
        _upgradeName.text = data.name;
        if (data.colorUpgrade != null)
            _onColorUpgradeChoose.Invoke(data.colorUpgrade);
        else if (data.lights)
            _onLightUpgradeChoose.Invoke();
        else
            _onSimpleUpgradeChoose.Invoke();
    }
}
