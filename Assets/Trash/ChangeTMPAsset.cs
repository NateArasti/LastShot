using TMPro;
using UnityEditor;
using UnityEngine;

public class ChangeTMPAsset : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset _asset;

    [ContextMenu("Change")]
    public void Change()
    {
        foreach (var text in GetComponentsInChildren<TMP_Text>(includeInactive:true))
        {
            text.font = _asset;
#if UNITY_EDITOR
            EditorUtility.SetDirty(text);
#endif
        }
    }
}
