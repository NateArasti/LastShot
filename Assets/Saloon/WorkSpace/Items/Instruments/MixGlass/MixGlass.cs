using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PourItem))]
public class MixGlass : MonoBehaviour, IWorkItem
{
    [SerializeField] private UnityEvent OnFix;
    [SerializeField] private float _delayBetweenDrops = 0.05f;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private RectTransform _mask;
    [SerializeField] private RectTransform _textureTransform;
    private Liquid _liquid;
    private PourItem _pourItem;

    private void Start()
    {
        _pourItem = GetComponent<PourItem>();
        _textureTransform.transform.position = Vector3.zero;
        transform.SetAsFirstSibling();
        _liquid = OrderCreationEvents.Instance.SpawnStartLiquid(_mask);
        transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<ItemSpace>().ConnectLiquid(_liquid);
    }

    public Sprite Sprite => _sprite;

    public bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type) => type != ItemSpace.ItemSpaceType.Glass;

    public GameObject SpawnWorkItem(Transform container) => Instantiate(gameObject, container);

    public bool TakeMousePosition() => false;

    public void TryFixGlass()
    {
        if(_liquid.TryGetColorAfterStir(out var colors))
        {
            Destroy(_liquid.gameObject);
            _pourItem.enabled = true;
            _pourItem.SetItem(_sprite, colors, _delayBetweenDrops);
            OnFix.Invoke();
        }
    }
}
