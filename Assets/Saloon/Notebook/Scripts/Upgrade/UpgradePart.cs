using System.Collections.Generic;
using UnityEngine;

public class UpgradePart : MonoBehaviour
{
    [SerializeField] private bool _light;

    private List<(GameObject upgrade, bool normalActive, ColorUpgrade colorUpgrade)> _upgrades = 
        new List<(GameObject upgrade, bool normalActive, ColorUpgrade colorUpgrade)>();

    private int _currentUpgradeIndex;

    private void Awake()
    {
        for(var i = 0; i < transform.childCount; ++i)
        {
            var upgrade = transform.GetChild(i).gameObject;
            _upgrades.Add((upgrade, upgrade.activeSelf, upgrade.GetComponent<ColorUpgrade>()));
        }
    }

    public (bool isOn, string name, ColorUpgrade colorUpgrade, bool lights) GetCurrentUpgradeData() => 
        (_upgrades[_currentUpgradeIndex].upgrade.activeSelf, 
        _upgrades[_currentUpgradeIndex].upgrade.name,
        _upgrades[_currentUpgradeIndex].colorUpgrade,
        _light);

    public void SwitchToNext() => Switch(true);

    public void SwitchToPrevious() => Switch(false);

    private void Switch(bool toNext)
    {
        if (toNext)
        {
            if (_currentUpgradeIndex == _upgrades.Count - 1)
                _currentUpgradeIndex = 0;
            else
                _currentUpgradeIndex++;
        }
        else
        {
            if (_currentUpgradeIndex == 0)
                _currentUpgradeIndex = _upgrades.Count - 1;
            else
                _currentUpgradeIndex--;
        }

        ShowUpgrade(true);
    }

    public void ReturnToStart()
    {
        for (var i = 0; i < _upgrades.Count; ++i)
        {
            _upgrades[i].upgrade.SetActive(_upgrades[i].normalActive);
            if (_upgrades[i].normalActive)
                _currentUpgradeIndex = i;
        }
    }

    public void ConsolidateUpgrade()
    {
        for (var i = 0; i < _upgrades.Count; ++i)
        {
            _upgrades[i] = (_upgrades[i].upgrade, false, _upgrades[i].colorUpgrade);
        }
        _upgrades[_currentUpgradeIndex] = 
            (_upgrades[_currentUpgradeIndex].upgrade, 
            _upgrades[_currentUpgradeIndex].upgrade.activeSelf, 
            _upgrades[_currentUpgradeIndex].colorUpgrade);
    }

    public void ShowUpgrade(bool show)
    {
        if (show)
        {
            foreach (var upgrade in _upgrades)
                upgrade.upgrade.SetActive(false);
            _upgrades[_currentUpgradeIndex].upgrade.SetActive(true);
        }
        else
        {
            for (var i = 0; i < _upgrades.Count; ++i)
                _upgrades[i].upgrade.SetActive(_upgrades[i].normalActive);
        }
    }
}