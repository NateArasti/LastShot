using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Animation))]
public class ShelfBottle : MonoBehaviour
{
    public enum PanelShowDirection
    {
        Top,
        Down,
        Right,
        Left
    }

    [SerializeField] private PanelShowDirection _direction;
    [SerializeField] private RectTransform _panelRect;
    [Header("Texts")]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _vPlace;
    [SerializeField] private TMP_Text _bottleLeftPlace;

    private Image _image;
    private UnityAction _onClick;
    private Animation _incorrectChoose;

    private void Awake()
    {
        _incorrectChoose = GetComponent<Animation>();
        _image = GetComponent<Image>();
        _image.enabled = false;
        _panelRect.gameObject.SetActive(false);
        switch (_direction)
        {
            case PanelShowDirection.Down:
                _panelRect.pivot = new Vector2(0.5f, 1.75f);
                break;
            case PanelShowDirection.Top:
                _panelRect.pivot = new Vector2(0.5f, -0.75f);
                break;
            case PanelShowDirection.Left:
                _panelRect.pivot = new Vector2(-0.75f, 0.5f);
                break;
            case PanelShowDirection.Right:
                _panelRect.pivot = new Vector2(1.75f, 0.5f);
                break;
        }
    }

    public void SetAlcohol(Ingredient alcohol, System.Func<Ingredient, ShelfBottle, bool> isCorrectChoose)
    {
        _image.sprite = alcohol.Icon;
        _image.enabled = true;
        var data = alcohol.Data;
        _name.text = data.Name;
        _vPlace.text = data.BuyQuantityStep.ToString();
        _bottleLeftPlace.text = "0";

        _onClick += () =>
        {
            HidePanel();
            if (isCorrectChoose.Invoke(alcohol, this))
            {
                gameObject.SetActive(false);
            }
            else
            {
                _incorrectChoose.Play();
            }
        };
    }

    public void Unchoose() => gameObject.SetActive(true);

    public void ShowPanel() => _panelRect.gameObject.SetActive(true);

    public void HidePanel() => _panelRect.gameObject.SetActive(false);

    public void OnClick() => _onClick.Invoke();
}
