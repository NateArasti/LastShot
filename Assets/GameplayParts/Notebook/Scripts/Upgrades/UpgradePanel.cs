using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradePanel : MonoBehaviour
{
    public enum ChooseType
    {
        Select,
        Multiple
    }

    [SerializeField] private ChooseType _chooseType;
    [SerializeField] private string _name;
    [SerializeField] private UpgradeData[] _upgradeDatas;
    [SerializeField] private UnityEvent<ChooseType, string, UpgradeData[]> _onChooseEvent;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => _onChooseEvent.Invoke(_chooseType, _name, _upgradeDatas));
    }

    [System.Serializable]
    public class UpgradeData
    {
        public enum State
        {
            Selected,
            Unselected,
            Discount,
            Buy
        }

        [SerializeField] private string _keyName;
        [SerializeField] private string _name;
        [SerializeField] private State _state;
        [SerializeField] private bool _locked;
        [SerializeField] private bool _containsInfo;
        [SerializeField] private int _price;
        [SerializeField] private bool _onDiscount;

        public string KeyName => _keyName;

        public string Name => _name;

        public State UpgradeState
        {
            get => _state;
            set => _state = value;
        }

        public bool ContainsInfo => _containsInfo;

        public int Price => _price;

        public bool OnDiscount => _onDiscount;

        public bool Locked => _locked;
    }
}
