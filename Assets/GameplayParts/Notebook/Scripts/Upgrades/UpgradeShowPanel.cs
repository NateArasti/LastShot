using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeShowPanel : MonoBehaviour
{
    public enum ChooseType
    {
        Select,
        Multiple
    }

    [SerializeField] private ChooseType _chooseType;
    [SerializeField] private string _name;
    [SerializeField] private UpgradeShowData[] _upgradeDatas;
    [SerializeField] private UnityEvent<ChooseType, string, UpgradeShowData[]> _onChooseEvent;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _upgradeDatas.ForEachAction(data => data.Toggle());
        _button.onClick.AddListener(() => _onChooseEvent.Invoke(_chooseType, _name, _upgradeDatas));
    }

    [System.Serializable]
    public class UpgradeShowData
    {
        [SerializeField] private string _name;
        [SerializeField] private bool _shown;
        [SerializeField] private UnityEvent<bool> _toggleEvent;
        [SerializeField] private UnityEvent<bool> _inverseToggleEvent;

        public string Name => _name;

        public bool Shown
        {
            get => _shown;
            set => _shown = value;
        }

        public void Toggle()
        {
            _toggleEvent.Invoke(_shown);
            _inverseToggleEvent.Invoke(!_shown);
        }
    }
}
