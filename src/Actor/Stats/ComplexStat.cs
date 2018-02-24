using Autrage.LEX.NET.Serialization;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

#endif

[DataContract]
public sealed class ComplexStat : Stat, IExtendable<ComplexStat>
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
    private StatSum basis;

    [SerializeField]
    [DataMember]
    private StatProduct multiplier;

    [SerializeField]
    [DataMember]
    private StatSum addend;

    [SerializeField]
    [DataMember]
    private StatSum min;

    [SerializeField]
    [DataMember]
    private StatSum max;

    public StatSum Basis => basis;
    public StatProduct Multiplier => multiplier;
    public StatSum Addend => addend;
    public StatSum Min => min;
    public StatSum Max => max;

    public override float Value => min == null || Max == null || Min > Max ? Actual : Mathf.Clamp(Actual, Min, Max);
    public float Actual => BaseVal * MultVal + AddVal;

    public float DefaultValue => defaultMin > defaultMax ? DefaultActual : Mathf.Clamp(DefaultActual, defaultMin, defaultMax);
    public float DefaultActual => defaultBasis * defaultMultiplier + defaultAddend;

    public float Improvement => Value / DefaultValue;
    public float ActualImprovement => Actual / DefaultActual;

    private float BaseVal => Basis ?? 0f;
    private float MultVal => Multiplier ?? 1f;
    private float AddVal => Addend ?? 0f;

    public void Extend(ComplexStat other)
    {
        basis.Add(other.basis.ToArray());
        multiplier.Add(other.multiplier.ToArray());
        addend.Add(other.addend.ToArray());
    }

    protected override void Incorporate() => Owner.Complices.Add(this);

    protected override void Excorporate() => Owner.Complices.Remove(this);

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