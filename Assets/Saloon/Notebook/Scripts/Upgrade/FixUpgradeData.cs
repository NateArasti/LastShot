using UnityEngine;
using UnityEngine.Events;

public class FixUpgradeData : MonoBehaviour
{
    [SerializeField] private UnityEvent _onFix;
    [SerializeField] private string _consolidateName;

    public UnityEvent OnFix => _onFix;
    public string ConsolidateName => _consolidateName;
    public bool Fixed { get; set; }

    private void Awake()
    {
        Fixed = gameObject.activeSelf;
    }
}
