using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class LiquidDrop : MonoBehaviour
{
    public float CurrentVelocity => 0.005f * Mass * _rigidbody2D.velocity.magnitude;
    public float Mass { get; private set; }
    public int LiquidIndex { get; private set; }
    public (Color mainColor, Color outlineColor) Colors { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetParams(float scale, float mass, Vector2 velocity, (Color mainColor, Color outlineColor) colors, int index)
    {
        LiquidIndex = index;
        Mass = mass;
        transform.localScale = Vector3.one * scale;
        _rigidbody2D.velocity = velocity;
        _spriteRenderer.color = colors.mainColor;
        Colors = colors;
    }

    public void Die()
    {
        _spriteRenderer.enabled = false;
        Destroy(gameObject, 0.01f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Finish"))
            Destroy(gameObject);
    }
}
