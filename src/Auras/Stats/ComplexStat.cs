using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

[Serializable]
[DataContract]
public class ComplexStat : Stat
{
    [SerializeField]
    [DataMember]
    private float defaultBasis;

    [SerializeField]
    [DataMember]
    private float defaultMultiplier = 1f;

    [SerializeField]
    [DataMember]
    private float defaultAddend;

    [SerializeField]
    [DataMember]
    private float defaultMin;

    [SerializeField]
    [DataMember]
    private float defaultMax;

    [DataMember]
    public Stat Basis { get; set; }

    [DataMember]
    public Stat Multiplier { get; set; }

    [DataMember]
    public Stat Addend { get; set; }

    [DataMember]
    public Stat Min { get; set; }

    [DataMember]
    public Stat Max { get; set; }

    public override float Value => Min == null || Max == null || Min > Max ? Actual : Mathf.Clamp(Actual, Min, Max);
    public float Actual => BaseVal * MultVal + AddVal;

    public float DefaultValue => defaultMin > defaultMax ? DefaultActual : Mathf.Clamp(DefaultActual, defaultMin, defaultMax);
    public float DefaultActual => defaultBasis * defaultMultiplier + defaultAddend;

    public float Improvement => Value / DefaultValue;
    public float ActualImprovement => Actual / DefaultActual;

    private float BaseVal => Basis ?? 0f;
    private float MultVal => Multiplier ?? 1f;
    private float AddVal => Addend ?? 0f;

    public ComplexStat()
    {
        Basis = defaultBasis;
        Multiplier = defaultMultiplier;
        Addend = defaultAddend;
        Min = defaultMin;
        Max = defaultMax;
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ComplexStat))]
    public class ComplexStatDrawer : PropertyDrawer
    {
        private const float xLabelWidth = 15f;
        private const float plusLabelWidth = 15f;
        private const float elementOfLabelWidth = 30f;
        private const float semicolonLabelWidth = 8f;
        private const float closingBracketLabelWidth = 8f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            float fieldWidth = (position.width - xLabelWidth - plusLabelWidth - elementOfLabelWidth - semicolonLabelWidth - closingBracketLabelWidth) / 5f;

            // Temporarily remove indent for lists and sub-properties
            EditorGUI.indentLevel = 0;

            position.width = fieldWidth;
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(defaultBasis)), GUIContent.none);

            position.x += fieldWidth;
            position.width = xLabelWidth;
            GUI.Label(position, "✕", new GUIStyle() { alignment = TextAnchor.MiddleCenter });

            position.x += xLabelWidth;
            position.width = fieldWidth;
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(defaultMultiplier)), GUIContent.none);

            position.x += fieldWidth;
            position.width = plusLabelWidth;
            GUI.Label(position, "+", new GUIStyle() { alignment = TextAnchor.MiddleCenter });

            position.x += plusLabelWidth;
            position.width = fieldWidth;
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(defaultAddend)), GUIContent.none);

            position.x += fieldWidth;
            position.width = elementOfLabelWidth;
            GUI.Label(position, "∈[ ", new GUIStyle() { alignment = TextAnchor.MiddleRight });

            position.x += elementOfLabelWidth;
            position.width = fieldWidth;
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(defaultMin)), GUIContent.none);

            position.x += fieldWidth;
            position.width = semicolonLabelWidth;
            GUI.Label(position, ";", new GUIStyle() { alignment = TextAnchor.MiddleCenter });

            position.x += semicolonLabelWidth;
            position.width = fieldWidth;
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(defaultMax)), GUIContent.none);

            position.x += fieldWidth;
            position.width = closingBracketLabelWidth;
            GUI.Label(position, " ]", new GUIStyle() { alignment = TextAnchor.MiddleLeft });

            EditorGUI.EndProperty();
        }
    }

#endif
}