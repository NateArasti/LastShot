using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ScrollContentFiller))]
public class DialogueDrinkScroll : MonoBehaviour
{
    [SerializeField] private DialogueDrinkPanel _panelPrefab;
    [SerializeField] private UnityEvent<Drink> _chooseEvent;
    private ScrollContentFiller _contentFiller;

    private void Awake() => _contentFiller = GetComponent<ScrollContentFiller>();

    private void Start()
    {
        _contentFiller.FillContent(
            _panelPrefab,
            DatabaseManager.DrinkDatabase.GetObjectsCollection(),
            (panel, drink) => panel.SetData(drink, _chooseEvent));
    }
}
