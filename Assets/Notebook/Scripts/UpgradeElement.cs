using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UpgradeElement : MonoBehaviour
{
    [SerializeField] private UpgradeSprites[] _sprites;
    [SerializeField] private Sprite _discountSprite;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _discountPrice;
    [SerializeField] private GameObject _lockGameObject;
    [SerializeField] private Image _panelImage;
    [SerializeField] private Button _button;
    private Image _referenceImage;
    private UpgradePanel.UpgradeData _data;

    private void Awake()
    {
        _referenceImage = GetComponent<Image>();
    }

    public void SetData(
        UpgradePanel.UpgradeData data,
        UnityAction<string, UpgradePanel.UpgradeData.State> onChangeStateAction)
    {
        _data = data;
        _sprites.ForEachAction(state =>
        {
            if (state.State == data.UpgradeState)
            {
                _panelImage.sprite = state.Sprite;
                _referenceImage.sprite = state.Sprite;
            }
        });
        if (data.UpgradeState is UpgradePanel.UpgradeData.State.Buy or
            UpgradePanel.UpgradeData.State.Locked)
        {
            _price.enabled = true;
            _price.text = $"{data.Price}$";

            if (data.OnDiscount)
            {
                _panelImage.sprite = _discountSprite;
                _referenceImage.sprite = _discountSprite;
                _price.fontStyle = FontStyles.Strikethrough;
                var priceColor = _price.color;
                priceColor.a = 0.15f;
                _price.color = priceColor;
                _discountPrice.text = $"{Mathf.CeilToInt(0.5f * data.Price)}$";
                _discountPrice.enabled = true;
            }
        }
        _name.text = data.Name;
        _lockGameObject.SetActive(data.UpgradeState == UpgradePanel.UpgradeData.State.Locked);
        _button.onClick.AddListener(() =>
            onChangeStateAction.Invoke(_data.KeyName, UpgradePanel.UpgradeData.State.Selected));
        
    }

    [System.Serializable]
    public struct UpgradeSprites
    {
        [SerializeField] private UpgradePanel.UpgradeData.State _state;
        [SerializeField] private Sprite _sprite;

        public UpgradePanel.UpgradeData.State State => _state;

        public Sprite Sprite => _sprite;
    }
}