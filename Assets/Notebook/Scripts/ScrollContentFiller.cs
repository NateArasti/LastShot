using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScrollContentFiller : MonoBehaviour
{
    [SerializeField] private Transform _content;

    public IReadOnlyDictionary<T2, T1> FillContent<T1, T2>(
        T1 contentItemPrefab, 
        IReadOnlyCollection<T2> collection,
        UnityAction<T1, T2> spawnEvent,
        bool destroyChildren = false) where T1 : MonoBehaviour
    {
        if (destroyChildren)
        {
            _content.DestroyChildren();
        }

        var spawnedObjects = new Dictionary<T2, T1>();

        collection.ForEachAction(arg1 =>
        {
            var arg0 = Instantiate(contentItemPrefab, _content);
            spawnedObjects.Add(arg1, arg0);
            spawnEvent.Invoke(arg0, arg1);
        });

        return spawnedObjects;
    }
}
