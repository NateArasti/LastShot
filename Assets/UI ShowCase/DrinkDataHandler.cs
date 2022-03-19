using UnityEngine;
using UnityEngine.Events;

public class DrinkDataHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent<Sprite> _iconEvent;
    [SerializeField] private UnityEvent<string> _nameEvent;
    [SerializeField] private UnityEvent<string> _descriptionEvent;
    private int _index;

    public UnityEvent<int> OnDrinkChoose { get; set; } = new();

    public void SetDrinkData(Drink drink, int index)
    {
        _index = index;
        var infoData = drink.InfoData;
        _iconEvent.Invoke(infoData.icon);
        _nameEvent.Invoke(infoData.name);
        _descriptionEvent.Invoke(infoData.description);
    }

    public void Choose()
    {
        OnDrinkChoose.Invoke(_index);
    }

    private void OnDestroy()
    {
        OnDrinkChoose.RemoveAllListeners();
    }
}
