using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(
    typeof(CanvasGroup), 
    typeof(RectTransform),
    typeof(ContentSizeFitter))]
public class BarShelfTooltip : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    private static BarShelfTooltip _instance;
    private Coroutine _showCoroutine;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private ContentSizeFitter _sizeFitter;

    private void Awake()
    {
        _instance = this;
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _sizeFitter = GetComponent<ContentSizeFitter>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _canvasGroup.alpha = 0;
        var mousePosition = Input.mousePosition;
        _rectTransform.pivot = new Vector2(mousePosition.x / Screen.width, mousePosition.y / Screen.height);
        transform.position = mousePosition;
        _sizeFitter.SetLayoutHorizontal();
        _sizeFitter.SetLayoutVertical();
    }


    public static void TryShow(string name)
    {
        _instance._showCoroutine = _instance.StartCoroutine(TooltipShow());
        IEnumerator TooltipShow()
        {
            yield return UnityExtensions.Wait(0.5f);
            _instance._name.text = name;
            _instance._canvasGroup.alpha = 1;
            _instance._name.gameObject.SetActive(true);
        }
    }

    public static void Hide()
    {
        if(_instance._showCoroutine != null) _instance.StopCoroutine(_instance._showCoroutine);
        _instance._canvasGroup.alpha = 0;
        _instance._name.gameObject.SetActive(false);
    }
}
