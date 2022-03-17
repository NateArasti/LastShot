using UnityEngine;
using UnityEngine.UI;

public class PageSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _pages;
    [SerializeField] private Image[] _bookmarks;

    private void Start()
    {
        SwitchToPage(_pages[0]);
        SwitchBookmark(_bookmarks[0]);
    }

    public void SwitchToPage(GameObject chosenPage)
    {
        foreach(var page in _pages)
        {
            page.SetActive(false);
        }
        chosenPage.SetActive(true);
    }

    public void SwitchBookmark(Image chosenBookmark)
    {
        foreach (var bookmark in _bookmarks)
        {
            Color.RGBToHSV(bookmark.color, out var h, out var _, out var v);
            bookmark.color = Color.HSVToRGB(h, 0.4f, v);
            bookmark.transform.GetChild(0).gameObject.SetActive(false);
        }

        Color.RGBToHSV(chosenBookmark.color, out var H, out var _, out var V);
        chosenBookmark.color = Color.HSVToRGB(H, 0.7f, V);
        chosenBookmark.transform.GetChild(0).gameObject.SetActive(true);
    }
}
