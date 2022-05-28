using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(OrderAction))]
public class OrderActionDrawer : PropertyDrawer
{
    private Dictionary<OrderAction.ActionType, (string keyName, string showName)[]> _properties = 
        new Dictionary<OrderAction.ActionType, (string keyName, string showName)[]>()
        {
            [OrderAction.ActionType.Add] = new (string keyName, string showName)[]
            {
                ("_ingredient", "Ingredient"),
                ("_quantity", "Quantity")
            },

            [OrderAction.ActionType.Mix] = new (string keyName, string showName)[] 
            {
                ("_timeAmout", "Time"),
                ("_intensity", "Intensity")
            },

            [OrderAction.ActionType.None] = new (string keyName, string showName)[]
            {
            }
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        var contentPosition = EditorGUI.PrefixLabel(position, new GUIContent($"Action {label.text.Split(' ')[1]}"));
        contentPosition.width *= 0.25f;
        contentPosition.x -= 1.5f * contentPosition.width;
        var actionType = property.FindPropertyRelative("_currentActionType");
        EditorGUI.PropertyField(contentPosition, actionType, GUIContent.none);

        contentPosition.x += 1.5f * contentPosition.width;
        var subProperties = _properties[(OrderAction.ActionType)actionType.enumValueIndex];
        var delta = (position.x + position.width - contentPosition.x) / (2.5f * subProperties.Length);
        contentPosition.width = delta;
        foreach (var subProperty in subProperties)
        {
            EditorGUI.LabelField(contentPosition, new GUIContent(subProperty.showName));
            contentPosition.x += contentPosition.width;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative(subProperty.keyName), GUIContent.none);
            contentPosition.x += 1.4f * contentPosition.width;
        }

        EditorGUI.EndProperty();
    }
}