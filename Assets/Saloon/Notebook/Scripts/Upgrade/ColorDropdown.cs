using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class ColorDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
    }

    public void SetDropdown(ColorUpgrade colorUpgrade)
    {
        _dropdown.ClearOptions();
        _dropdown.AddOptions(colorUpgrade.ColorNames);
        _dropdown.onValueChanged.AddListener(colorUpgrade.SetColor);
        _dropdown.onValueChanged.Invoke(colorUpgrade.CurrentIndex);
    }
}
