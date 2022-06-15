using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class LiquidTrigger : MonoBehaviour
{
    public enum LiquidContainerType
    {
        Glass,
        Shaker
    }

    public LiquidContainerType ContainerType { get; set; }

    [SerializeField] private float _triggerTopOffset = 0.1f;
    [SerializeField] private AudioClip _dropClip;

    public readonly UnityEvent<float, float, float, bool> OnHit = new();
    public readonly UnityEvent<Color> ReColor = new();

    private BoxCollider2D _boxCollider;

    private bool _smthLies;

    private OrderAction.IngredientAddAction _currentWaterDropAction;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var x = col.transform.position.x;
        if (col.TryGetComponent<DropItem>(out var dropItem))
        {
            OnHit.Invoke(x, dropItem.Force, dropItem.Volume, true);
            AudioManager.PlaySound(_dropClip, 0.2f);
        }
        else if (col.TryGetComponent<WaterDrop>(out var waterDrop))
        {
            ReColor.Invoke(waterDrop.DropColor);
            var waterDropMass = waterDrop.CurrentMass;

            if (_smthLies)
                OnHit.Invoke(x, 0.1f, waterDropMass * 2, false); // доп увеличения объема воды при соприкосновении с кубиком
            else
                OnHit.Invoke(x, 0.1f, waterDropMass, false);

            if (_currentWaterDropAction != null && 
                PourItem.PouringItemKeyName != _currentWaterDropAction.Ingredient.KeyName && 
                PourItem.PouringItemKeyName != string.Empty)
            {
                _currentWaterDropAction = null;
            }
            if (_currentWaterDropAction == null && PourItem.PouringItemKeyName != string.Empty)
            {
                _currentWaterDropAction = ContainerType == LiquidContainerType.Glass ?
                    new OrderAction.IngredientAddAction(false) :
                    new OrderAction.IngredientAddToShakerAction(false);
                if (DatabaseManager.AdditionalIngredientDatabase.TryGetValue(PourItem.PouringItemKeyName,
                        out var additionalIngredient))
                {
                    _currentWaterDropAction.Ingredient = additionalIngredient;
                }
                else if (DatabaseManager.AlcoholDatabase.TryGetValue(PourItem.PouringItemKeyName,
                        out var alcohol))
                {
                    _currentWaterDropAction.Ingredient = alcohol;
                }
                else Debug.LogError($"WTF ?? {PourItem.PouringItemKeyName}");
                if(ContainerType == LiquidContainerType.Glass)
                    OrderCreationEvents.Instance.OrderActionsTracker.AddAction(_currentWaterDropAction);
                else
                    OrderCreationEvents.Instance.OrderActionsTracker.AddAction(
                        _currentWaterDropAction as OrderAction.IngredientAddToShakerAction
                        );
            }
            _currentWaterDropAction.Quantity += waterDropMass / PourIngredient.VolumeToWaterDrop;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<DropItem>(out var mass)) _smthLies = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<DropItem>(out var mass)) _smthLies = true;
    }

    public void SetTriggerBounds(float width, float topPosition, float xOffset = 0)
    {
        var height = _boxCollider.size.y;
        _boxCollider.size = new Vector2(width, height);
        _boxCollider.offset =
            new Vector2(xOffset == 0 ? _boxCollider.offset.x : xOffset, 
                topPosition - height / 2 + _triggerTopOffset);
    }
}