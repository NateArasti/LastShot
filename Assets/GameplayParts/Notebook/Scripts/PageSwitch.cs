using UnityEngine;
using UnityEngine.Events;

public class PageSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private int _startIndex;
    [SerializeField] private UnityEvent _onPageSwitch;

    public int CurrentPageIndex { get; private set; }
    public int PagesCount => _pages.Length;

    private void Start()
    {
        foreach (var page in _pages)
        {
            page.SetActive(true);
        }
        OpenPage(_startIndex);
        CurrentPageIndex = _startIndex;
    }

    public void OpenPage(int pageID)
    {
        if(pageID > _pages.Length || pageID < 0) throw new UnityException($"ID out of range({_pages.Length}");
        _pages.ForEachAction(page => page.SetActive(false));
        _pages[pageID].SetActive(true);
        CurrentPageIndex = pageID;
        _onPageSwitch.Invoke();
    }

    public void ShiftPage(int delta)
    {
        var newIndex = CurrentPageIndex + delta;
        while (newIndex < 0) newIndex += PagesCount;
        newIndex %= PagesCount;
        OpenPage(newIndex);
    }
}
