using UnityEngine;

[System.Serializable]
public class OrderAction
{
    public enum ActionType
    {
        None,
        Add,
        Mix
    }

    [SerializeField] private ActionType _currentActionType;

    public ActionType CurrentActionType => _currentActionType;

    #region Add
    [SerializeField] private Ingredient _ingredient;
    [SerializeField] private int _quantity;
    #endregion

    #region Mix
    [SerializeField] private float _timeAmout;
    [SerializeField] private float _intensity;
    #endregion

    public bool Compare(OrderAction comparableAction)
    {
        if(_currentActionType != comparableAction._currentActionType) return false;

        switch (_currentActionType)
        {
            case ActionType.Add:
                return _ingredient == comparableAction._ingredient &&
                    _quantity == comparableAction._quantity;
            case ActionType.Mix:
                return _timeAmout == comparableAction._timeAmout &&
                    _intensity == comparableAction._intensity;
            case ActionType.None:
                return true;
            default:
                return false;
        }
    }
}