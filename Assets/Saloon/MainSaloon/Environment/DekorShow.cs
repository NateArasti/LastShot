using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DekorShow : MonoBehaviour
{
    [SerializeField] private LightShow _lightShow;
    [SerializeField] private float _delay = 1f;
    private List<GameObject> _kids = new List<GameObject>();

    private void Start()
    {
        _lightShow.SetLight(1);
        for(var i = 0; i < transform.childCount; ++i)
        {
            _kids.Add(transform.GetChild(i).gameObject);
        }
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        foreach(var kid in _kids)
        {
            yield return new WaitForSeconds(_delay);
            kid.SetActive(true);
        }
    }
}
