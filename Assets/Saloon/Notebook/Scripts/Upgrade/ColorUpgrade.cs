using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorUpgrade : MonoBehaviour
{
    [SerializeField] private ColorData[] _colorDatas;
    private SpriteRenderer _spriteRenderer;

    public int CurrentIndex { get; private set; }
    public List<string> ColorNames => _colorDatas.Select(data => data.Name).ToList();

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(int index)
    {
        CurrentIndex = index;
        _spriteRenderer.color = _colorDatas[index].Color;
    }
}

[System.Serializable]
public struct ColorData
{
    [SerializeField] private string _name;
    [SerializeField] private Color _color;

    public string Name => _name;
    public Color Color => _color;
}
