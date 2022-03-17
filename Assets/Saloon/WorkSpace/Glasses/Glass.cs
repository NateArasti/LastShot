using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Glass : MonoBehaviour
{
    [SerializeField] private float _v;
    [Space(20)]
    [SerializeField] private RectTransform _textureTransform;
    [SerializeField] private Image _glassImage;
    [SerializeField] private Image _glassMask;
    [SerializeField] private GarnishSpace[] _garnishSpaces;
    
    public float V => _v;

    private void Start()
    {
        GetComponent<Image>().SetNativeSize();
        _glassImage.SetNativeSize();
        _glassMask.SetNativeSize();
        _textureTransform.position = Vector3.zero;
        var liquid = OrderCreationEvents.Instance.SpawnStartLiquid(_glassMask.rectTransform);
        transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<ItemSpace>().ConnectLiquid(liquid);
        ItemSpacesStorage.ConnectGarnsihSpaces(_garnishSpaces);
        var garnishSpacesParent = _garnishSpaces[0].transform.parent;
        garnishSpacesParent.SetParent(transform.parent);
        garnishSpacesParent.SetAsLastSibling();
    }
}
