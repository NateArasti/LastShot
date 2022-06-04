using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class LiquidTrigger : MonoBehaviour
{
    [SerializeField] private float _triggerTopOffset = 0.1f;
    //public Color Color;

    public readonly UnityEvent<float, float, float> OnHit = new();
    public readonly UnityEvent<Color> ReColor = new();

    private BoxCollider2D _boxCollider;

    private bool _smthLies;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var x = col.transform.position.x;
        if (col.TryGetComponent<DropItem>(out var dropItem))
        {
            OnHit.Invoke(x, dropItem.Force, dropItem.Volume);
        }
        else if (col.TryGetComponent<WaterDrop>(out var waterDrop))
        {
            ReColor.Invoke(waterDrop.DropColor);

            if (_smthLies)
                OnHit.Invoke(x, 0.1f, waterDrop.CurrentMass * 2); // доп увеличение объема воды при соприкосновении с кубиком
            else
                OnHit.Invoke(x, 0.1f, waterDrop.CurrentMass);
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