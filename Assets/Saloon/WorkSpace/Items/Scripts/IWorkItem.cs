using UnityEngine;

public interface IWorkItem
{
    bool CanPlaceInThisSpace(ItemSpace.ItemSpaceType type);
    bool TakeMousePosition();
    Sprite Sprite { get; }
    GameObject SpawnWorkItem(Transform container);
}
