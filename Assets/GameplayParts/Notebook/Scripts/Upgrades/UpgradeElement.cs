using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeElement : MonoBehaviour
{
    [SerializeField] private UpgradePanels[] _upgradePanels;
    [SerializeField] private TMP_Text[] _names;
    [SerializeField] private TMP_Text[] _prices;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private TMP_Text _discountPrice;
    [SerializeField] private GameObject _lockGameObject;
    private UpgradePanel.UpgradeData _data;

    public void SetData(
        UpgradePanel.UpgradeData data,
        UnityAction<string, UpgradePanel.UpgradeData.State> onChangeStateAction)
    {
        _data = data;
        _names.ForEachAction(t => t.text = _data.Name);
        _prices.ForEachAction(price => price.text = $"{data.Price}$"); 
        _discountPrice.text = $"{data.Price * 0.5f}$";
        _lockGameObject.SetActive(_data.Locked);
        _upgradePanels.ForEachAction(panel => panel.Panel.SetActive(panel.State == data.UpgradeState));

        _buttons.ForEachAction(button => 
            button.onClick.AddListener(() =>
                onChangeStateAction.Invoke(_data.KeyName, UpgradePanel.UpgradeData.State.Selected)
                )
            );
        
    }

    [System.Serializable]
    public struct UpgradePanels
    {
        [SerializeField] private UpgradePanel.UpgradeData.State _state;
        [SerializeField] private GameObject _panel;

        public UpgradePanel.UpgradeData.State State => _state;

        public GameObject Panel => _panel;
    }
}