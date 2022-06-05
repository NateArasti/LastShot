using UnityEngine;

public class InstrumentListItem : ListItem
{
    [SerializeField] private Instrument _instrument;
    public Instrument Instrument => _instrument;

    public override void Start()
    {
        _icon.sprite = _instrument.Icon;
        _dragItemIcon.sprite = _instrument.Icon;
    }
}