using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class ItemSpacesStorage : MonoBehaviour
{
    private static ItemSpacesStorage _instance;

    [SerializeField] private List<ItemSpace> _itemSpaces;
    [SerializeField] private Color _simpleColor;
    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _errorColor;
    private GarnishSpace[] _garnishSpaces;

    public static Canvas Canvas { get; private set; }
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            Canvas = GetComponent<Canvas>();
        }
        else
        {
            Debug.LogError("Another Spaces Storage!");
            Destroy(gameObject);
        }
    }

    public static void ConnectGarnishSpaces(ItemSpace[] garnishSpaces) => 
        _instance._itemSpaces.AddRange(garnishSpaces);

    public static void DisconnectGarnishSpaces()
    {
        foreach(var garnishSpace in _instance._garnishSpaces)
        {
            if (garnishSpace.Garnished) continue;
            garnishSpace.gameObject.SetActive(false);
        }
        _instance._garnishSpaces = Array.Empty<GarnishSpace>();
    }

    public static (Color simpleColor, Color highlightedColor, Color errorColor) GetColors() => 
        (_instance._simpleColor, _instance._highlightedColor, _instance._errorColor);

    public static void SetSpacesActive(bool active)
    {
        foreach(var space in _instance._itemSpaces)
        {
            if(active)
                space.Enable();
            else
                space.Disable();
            space.OnPointerExit(null);
        }
    }
}
