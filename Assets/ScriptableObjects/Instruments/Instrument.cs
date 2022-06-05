using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instrument", menuName = "Data/DataItem/Simple Instrument")]
public class Instrument : ScriptableObject, IDatabaseObject, ISpriteDatabaseObject
{
    [SerializeField] private string _keyName;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private ItemSpace.ItemSpaceType _allowableSpaceType;
    [SerializeField] private InstrumentInfoData _infoData;
    [SerializeField] private List<Drink> _containingDrinks;

    public List<Drink> ContainingDrinks
    {
        get => _containingDrinks;
        set => _containingDrinks = value;
    }

    public string KeyName => _keyName;

    public InstrumentInfoData Data => _infoData;

    public void WriteData(string[] paramsLine)
    {
        
    }

    public bool CanPlaceInSpace(ItemSpace.ItemSpaceType type) => _allowableSpaceType == type;

    [System.Serializable]
    public class InstrumentInfoData : InfoData
    {
    }

    public Sprite Icon
    {
        get => Data.ObjectSprite;
        set => Data.ObjectSprite = value;
    }

    public GameObject Prefab => _prefab;
}
