using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class LiquidTrigger : MonoBehaviour
{
    //public static BoxCollider2D _Box;

    public readonly UnityEvent<float, float> OnHit = new();

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        //_Box = _boxCollider;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var x = col.transform.position.x;
        if(col.TryGetComponent<DropItem>(out var mass))
        {
            OnHit.Invoke(x, mass.Mass);
            print("WQESADQWEQW");
        }
        else
        {
            OnHit.Invoke(x, WaterDrop.Mass);
        }
    }

    public void SetTriggerBounds(float width, float topPosition, float xOffset = 0)
    {
        var height = _boxCollider.size.y;
        _boxCollider.size = new Vector2(width, height);
        if (xOffset == 0)
            _boxCollider.offset = new Vector2(_boxCollider.offset.x, topPosition - height /2);
        else
            _boxCollider.offset = new Vector2(xOffset, topPosition - height /2);
    }

    //private IEnumerator DropCounter()
    //{

    //}
    //public static float GetForceMultiplier(float objectPos)
    //{
    //    return ((objectPos - _Box.offset.y ) / _Box.offset.y);
    //}
}