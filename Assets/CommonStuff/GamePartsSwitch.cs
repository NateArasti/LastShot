using UnityEngine;
using UnityEngine.Events;

public class GamePartsSwitch : MonoBehaviour
{
    private static GamePartsSwitch _instance;

    [SerializeField] private UnityEvent<Drink> _toBarShelf;
    [SerializeField] private UnityEvent<Drink> _toWorkSpace;
    [SerializeField] private UnityEvent _returnToSaloon;

    private void Start()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public static void StartCreatingOrder(Drink drink) => _instance._toBarShelf.Invoke(drink);

    public static void SwitchToWorkSpace(Drink drink) => _instance._toWorkSpace.Invoke(drink);

    public static void ComebackToSaloon() => _instance._returnToSaloon.Invoke();
}
