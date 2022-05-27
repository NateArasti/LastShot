using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Drink))]
public class DrinkEditor : Editor
{
    private SerializedProperty _receiptProperty;
    private string[] _orderActionsNames;
    private Type[] _orderActionsTypes;
    private int _currentAction;
    private Drink _drink;

    private SerializedProperty _costProperty;
    private float _primeCost;
    private static float _factor = 4;

    private void OnEnable()
    {
        _drink = target as Drink;
        _receiptProperty = serializedObject.FindProperty("_receipt");
        _costProperty = serializedObject.FindProperty("_infoData").FindPropertyRelative("_cost");
        _orderActionsTypes = (
            from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
            from assemblyType in domainAssembly.GetTypes()
            where typeof(OrderAction).IsAssignableFrom(assemblyType)
            select assemblyType
            ).Skip(1).ToArray();
        _orderActionsNames = _orderActionsTypes.Select(action => action.Name).ToArray();

        _primeCost = _drink.DrinkReceipt.CountPrimeCost();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10f);
        if(_receiptProperty.isExpanded)
        {
            EditorGUI.indentLevel++;
            _currentAction = EditorGUILayout.Popup("Action to add", _currentAction, _orderActionsNames);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add action", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
            {
                _drink.DrinkReceipt.AddAction(_orderActionsTypes[_currentAction]);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Remove Action", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
            {
                _drink.DrinkReceipt.RemoveAction();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.LabelField($"Prime cost: {_primeCost}$");
        _factor = EditorGUILayout.FloatField("Cost factor", _factor);
        EditorGUILayout.PropertyField(_costProperty);
        if (GUILayout.Button("Count cost"))
        {
            _primeCost = _drink.DrinkReceipt.CountPrimeCost();
            _costProperty.floatValue = _primeCost * _factor;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
