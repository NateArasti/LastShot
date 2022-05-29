using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeShowElement : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _showIcon;
    [SerializeField] private Button _button;

    public void SetData(UpgradeShowPanel.UpgradeShowData data, UnityAction<UpgradeShowPanel.UpgradeShowData> chooseAction)
    {
        _name.text = data.Name;
        _button.onClick.AddListener(() => chooseAction.Invoke(data));
    }

    public void SetIcon(bool shown) => _showIcon.gameObject.SetActive(shown);
}
