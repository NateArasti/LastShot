using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GrabDrop : MonoBehaviour
{
    public event UnityAction OnDestroy;

    public (Color color, float mass) Data { get; private set; }

    public bool Destructed { get; set; }

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Hide()
    {
        _image.color = new Color(0, 0, 0, 0);
    }

    public void Show()
    {
        _image.color = Data.color;
    }

    public void SetItem(Color color, float mass)
    {
        Data = (color, mass);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<LiquidTouchDetector>() != null)
            OnDestroy.Invoke();
    }
}
