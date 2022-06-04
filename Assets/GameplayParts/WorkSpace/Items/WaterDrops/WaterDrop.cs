using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaterDrop : MonoBehaviour
{
    public const float Mass = 0.05f;

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

    private float _currentMass = Mass;
    public float CurrentMass
    {
        get
        {
            var res = _currentMass;
            _currentMass = 0;
            return res;
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
        _currentMass = Mass;
    }
}