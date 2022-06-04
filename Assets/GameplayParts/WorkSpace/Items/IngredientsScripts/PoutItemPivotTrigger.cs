using UnityEngine;
using UnityEngine.Events;

public class PoutItemPivotTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _triggerEntered;
    [SerializeField] private UnityEvent _triggerExit;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("GlassSpace")) _triggerEntered.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GlassSpace")) _triggerExit.Invoke();
    }
}
