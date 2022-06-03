using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaterDrop : MonoBehaviour
{
    public const float Mass = 2f;

    public readonly UnityEvent<WaterDrop> KillAction = new();

    private Rigidbody2D _rigidbody;
    public Color DropColor { get; set; }

    public Vector2 Direction
    {
        set
        {
            if (_rigidbody == null) Awake();

            _rigidbody.velocity = value;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(KillDelay());
    }

    private IEnumerator KillDelay()
    {
        yield return UnityExtensions.Wait(0.01f);
        KillAction.Invoke(this);
        KillAction.RemoveAllListeners();
    }
}