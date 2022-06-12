using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Returner))]
public class ShakerGlass : MonoBehaviour
{
    [SerializeField] private float _v;
    [Space(20)] [SerializeField] private RectTransform _textureTransform;
    [SerializeField] private Image _glassImage;
    [SerializeField] private Image _glassMask;
    [SerializeField] private GameObject _liquidPrefab;
    [SerializeField] private Shaker _shaker;

    [SerializeField] private AnimationCurve _widthHeight;
    [Space(50)] [SerializeField] private Sprite _sprite;

    private UnityAction _endAction;
    private LiquidRenderer _liquidRenderer;
    private ItemSpace.ItemSpaceNumber _number;
    private StaticLiquid _liquid;

    public float V => _v;

    private void Start()
    {
        GetComponent<Image>().SetNativeSize();
        _glassImage.SetNativeSize();
        _glassMask.SetNativeSize();
        _textureTransform.position = Vector3.zero;

        var startLiquid = Instantiate(_liquidPrefab);
        startLiquid.gameObject.SetActive(true);
        _liquid = startLiquid.GetComponent<StaticLiquid>();
        _liquid.SpawnStartLiquid(_glassMask.rectTransform, _widthHeight, LiquidTrigger.LiquidContainerType.Shaker);
        _liquidRenderer = startLiquid.GetComponent<LiquidRenderer>();
        GetComponent<Returner>().OnReturn.AddListener(OnReturn);
    }

    private void OnDestroy()
    {
        Destroy(_liquid.gameObject);
    }

    public void OnReturn()
    {
        Instantiate(_shaker, transform.parent).SetUp(_liquidRenderer.CurrentGradient, _endAction, _number);
        Destroy(_liquidRenderer.gameObject);
    }

    public void SetUp(UnityAction endAction, ItemSpace.ItemSpaceNumber number)
    {
        _endAction = endAction;
        _number = number;
    }

    [ContextMenu("CalculateCurve")]
    private void CalculateCurve()
    {
        var pointsList = new List<(float height, float width)>();
        _widthHeight = new AnimationCurve();
        var texture = DuplicateTexture(_sprite.texture);
        var height = texture.height;
        var width = texture.width;
        var offset = 0;
        for (var i = 0; i < height; i++)
        {
            var pixelCount = 0;

            for (var j = 0; j < width; j++)
                if (texture.GetPixel(j, i) == Color.white)
                {
                    print(texture.GetPixel(j, i));
                    pixelCount++;
                }

            if (pixelCount == 0)
            {
                offset++;
                continue;
            }

            pointsList.Add((i - offset, pixelCount));
        }

        var maxHeight = pointsList.Max(x => x.height);
        var maxWidth = pointsList.Max(x => x.width);
        foreach (var valueTuple in pointsList)
            _widthHeight.AddKey(valueTuple.height / maxHeight, valueTuple.width / maxWidth);
    }

    private Texture2D DuplicateTexture(Texture2D source)
    {
        var renderTex = RenderTexture.GetTemporary(
            source.width,
            source.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        var previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        var readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
