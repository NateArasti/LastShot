using UnityEngine;

public class PageSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private int _startIndex;

    private void Start()
    {
        foreach (var page in _pages)
        {
            page.SetActive(true);
        }
        OpenPage(_startIndex);
    }

    public void OpenPage(int pageID)
    {
        if(pageID > _pages.Length || pageID < 0) throw new UnityException($"ID out of range({_pages.Length}");
        _pages.ForEachAction(page => page.SetActive(false));
        _pages[pageID].SetActive(true);
    }
}
