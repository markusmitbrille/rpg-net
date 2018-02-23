using Autrage.LEX.NET.Serialization;
using System;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

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

    [SerializeField]
    [DataMember]
    private Stat basis;

    [SerializeField]
    [DataMember]
    private Stat multiplier;

    [SerializeField]
    [DataMember]
    private Stat addend;

    [SerializeField]
    [DataMember]
    private Stat min;

    [SerializeField]
    [DataMember]
    private Stat max;

    public Stat Basis { get { return basis; } set { basis = value; } }
    public Stat Multiplier { get { return multiplier; } set { basis = multiplier; } }
    public Stat Addend { get { return addend; } set { basis = addend; } }
    public Stat Min { get { return min; } set { basis = min; } }
    public Stat Max { get { return max; } set { basis = max; } }

    public override float Value => min == null || Max == null || Min > Max ? Actual : Mathf.Clamp(Actual, Min, Max);
    public float Actual => BaseVal * MultVal + AddVal;

    public float DefaultValue => defaultMin > defaultMax ? DefaultActual : Mathf.Clamp(DefaultActual, defaultMin, defaultMax);
    public float DefaultActual => defaultBasis * defaultMultiplier + defaultAddend;

    public float Improvement => Value / DefaultValue;
    public float ActualImprovement => Actual / DefaultActual;

    private float BaseVal => Basis ?? 0f;
    private float MultVal => Multiplier ?? 1f;
    private float AddVal => Addend ?? 0f;

#if UNITY_EDITOR

    [CanEditMultipleObjects]
    [CustomEditor(typeof(ComplexStat))]
    public class ComplexStatEditor : Editor
    {
        private const float xLabelWidth = 15f;
        private const float plusLabelWidth = 15f;
        private const float elementOfLabelWidth = 30f;
        private const float semicolonLabelWidth = 8f;
        private const float closingBracketLabelWidth = 8f;

        private SerializedProperty defaultBasis;
        private SerializedProperty defaultMultiplier;
        private SerializedProperty defaultAddend;
        private SerializedProperty defaultMin;
        private SerializedProperty defaultMax;

        private SerializedProperty basis;
        private SerializedProperty multiplier;
        private SerializedProperty addend;
        private SerializedProperty min;
        private SerializedProperty max;

        private void OnEnable()
        {
            defaultBasis = serializedObject.FindProperty(nameof(ComplexStat.defaultBasis));
            defaultMultiplier = serializedObject.FindProperty(nameof(ComplexStat.defaultMultiplier));
            defaultAddend = serializedObject.FindProperty(nameof(ComplexStat.defaultAddend));
            defaultMin = serializedObject.FindProperty(nameof(ComplexStat.defaultMin));
            defaultMax = serializedObject.FindProperty(nameof(ComplexStat.defaultMax));

            basis = serializedObject.FindProperty(nameof(ComplexStat.basis));
            multiplier = serializedObject.FindProperty(nameof(ComplexStat.multiplier));
            addend = serializedObject.FindProperty(nameof(ComplexStat.addend));
            min = serializedObject.FindProperty(nameof(ComplexStat.min));
            max = serializedObject.FindProperty(nameof(ComplexStat.max));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(defaultBasis, GUIContent.none);
            GUILayout.Label("✕");
            EditorGUILayout.PropertyField(defaultMultiplier, GUIContent.none);
            GUILayout.Label("+");
            EditorGUILayout.PropertyField(defaultAddend, GUIContent.none);
            GUILayout.Label(":");
            EditorGUILayout.PropertyField(defaultMin, GUIContent.none);
            GUILayout.Label("< x <");
            EditorGUILayout.PropertyField(defaultMax, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(basis);
            EditorGUILayout.PropertyField(multiplier);
            EditorGUILayout.PropertyField(addend);
            EditorGUILayout.PropertyField(min);
            EditorGUILayout.PropertyField(max);
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}