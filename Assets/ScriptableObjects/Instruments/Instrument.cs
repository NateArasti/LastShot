using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instrument", menuName = "Data/DataItem/Simple Instrument")]
public class Instrument : ScriptableObject, IDatabaseObject, ISpriteDatabaseObject
{
    [SerializeField] private string _keyName;
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

    [System.Serializable]
    public class InstrumentInfoData : InfoData
    {
    }

    public Sprite Sprite
    {
        get => Data.ObjectSprite;
        set => Data.ObjectSprite = value;
    }
}
