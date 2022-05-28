using System;
using UnityEngine;
using UnityEngine.Events;

public class WaterDrop : MonoBehaviour
{
    public const float Mass = 0.3f;
    public Vector2 Direction
    {
        set
        {
            if (_rigidbody == null)
            {
                Awake();
            }

            _rigidbody.velocity = value;
        }
    }

    private Rigidbody2D _rigidbody;

    public readonly UnityEvent<WaterDrop> KillAction = new();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        KillAction.Invoke(this);
        KillAction.RemoveAllListeners();
    }
}
