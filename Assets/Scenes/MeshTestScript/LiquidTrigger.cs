using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class LiquidTrigger : MonoBehaviour
{
    //public static BoxCollider2D _Box;

    public readonly UnityEvent<float, float, float> OnHit = new();
    private bool _smthLies;
    private float _dropItemMass;

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        //_Box = _boxCollider;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //print(123);
        var x = col.transform.position.x;
        if(col.TryGetComponent<DropItem>(out var mass))
        {
            OnHit.Invoke(x, _dropItemMass / 2, _dropItemMass);
        }
        else if (_smthLies)
        {
            OnHit.Invoke(x, 0.1f,WaterDrop.Mass + 0.3f); 
        }
        else
        {
            OnHit.Invoke(x, 0.1f, WaterDrop.Mass);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _smthLies = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<DropItem>(out var mass))
        {
            _smthLies = false;
            print("!@#@!#!@#!@#");
        }
        //print(321);
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