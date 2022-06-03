using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Glass : MonoBehaviour
{
    [SerializeField] private float _v;

    [Space(20)] [SerializeField] private RectTransform _textureTransform;

    [SerializeField] private Image _glassImage;
    [SerializeField] private Image _glassMask;
    [SerializeField] private GarnishSpace[] _garnishSpaces;
    [SerializeField] private GameObject _liquidPrefab;

    [Space(50)] [SerializeField] private Sprite _sprite;

    [SerializeField] private AnimationCurve _widthHeight;

    public float V => _v;

    private void Start()
    {
        GetComponent<Image>().SetNativeSize();
        _glassImage.SetNativeSize();
        _glassMask.SetNativeSize();
        _textureTransform.position = Vector3.zero;

        var startLiquid = Instantiate(_liquidPrefab);
        startLiquid.gameObject.SetActive(true);
        startLiquid.GetComponent<StaticLiquid>().SpawnStartLiquid(_glassMask.rectTransform, _widthHeight);

        ItemSpacesStorage.ConnectGarnsihSpaces(_garnishSpaces);
        var garnishSpacesParent = _garnishSpaces[0].transform.parent;
        garnishSpacesParent.SetParent(transform.parent);
        garnishSpacesParent.SetAsLastSibling();
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