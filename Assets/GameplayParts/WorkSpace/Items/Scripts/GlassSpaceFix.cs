using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(RectTransform))]
public class GlassSpaceFix : MonoBehaviour
{
    [SerializeField] private float _additionalOffset = 50f;

    public void SetGlassSpaceSize(float glassTopPivot)
    {
        var rectTransform = GetComponent<RectTransform>();
        var boxCollider = GetComponent<BoxCollider2D>();
        var newHeight = 1080 - glassTopPivot - _additionalOffset;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
        boxCollider.size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        boxCollider.offset = new Vector2(0, -newHeight * 0.5f);
    }
}
