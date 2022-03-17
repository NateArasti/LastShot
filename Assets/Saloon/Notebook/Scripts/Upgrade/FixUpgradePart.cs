using UnityEngine;

public class FixUpgradePart : MonoBehaviour
{
    private FixUpgradeData[] _fixUpgradeDatas;
    private int _currentUpgradeIndex;

    public (bool isFixed, string name) GetCurrentUpgradeData() =>
        (_fixUpgradeDatas[_currentUpgradeIndex].Fixed,
        _fixUpgradeDatas[_currentUpgradeIndex].name);

    private void Start()
    {
        _fixUpgradeDatas = GetComponentsInChildren<FixUpgradeData>();
        for (var i = 0; i < _fixUpgradeDatas.Length; ++i)
        {
            _fixUpgradeDatas[i].gameObject.SetActive(!_fixUpgradeDatas[i].Fixed);
        }
    }

    public void SwitchToNext() => Switch(true);

    public void SwitchToPrevious() => Switch(false);

    private void Switch(bool toNext)
    {
        if (toNext)
        {
            if (_currentUpgradeIndex == _fixUpgradeDatas.Length - 1)
                _currentUpgradeIndex = 0;
            else
                _currentUpgradeIndex++;
        }
        else
        {
            if (_currentUpgradeIndex == 0)
                _currentUpgradeIndex = _fixUpgradeDatas.Length - 1;
            else
                _currentUpgradeIndex--;
        }

        ShowUpgrade(true);
    }

    public void ReturnToStart()
    {
        for (var i = 0; i < _fixUpgradeDatas.Length; ++i)
        {
            _fixUpgradeDatas[i].gameObject.SetActive(! _fixUpgradeDatas[i].Fixed);
        }
    }

    public void Fix()
    {
        _fixUpgradeDatas[_currentUpgradeIndex].Fixed = true;
    }

    public void ShowUpgrade(bool show)
    {
        ReturnToStart();
        _fixUpgradeDatas[_currentUpgradeIndex].gameObject.SetActive(show);
    }
}
