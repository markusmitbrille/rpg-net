using ProtoBuf;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// <para>Represents a composite stat that is calculated as a sum of bases, times a product of multipliers, plus a sum
/// of addends. Other stats constitute bases, multipliers and addends, making complex interrelated stats possible.</para>
/// <para><see cref="Value"/> is bounded by <see cref="min"/> and <see cref="max"/>, if they are set and valid, if not
/// it is equal to <see cref="Actual"/>.</para>
/// <para>Take care that references are not lost during serialization.</para>
/// </summary>
[Serializable]
[ProtoContract(AsReferenceDefault = true, SkipConstructor = true)]
public class ComplexStat : Stat
{
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

    // Default values

    [SerializeField]
    [ProtoMember(1)]
    private float defaultBasis = 0f;
    [SerializeField]
    [ProtoMember(2)]
    private float defaultMultiplier = 1f;
    [SerializeField]
    [ProtoMember(3)]
    private float defaultAddend = 0f;

    [SerializeField]
    [ProtoMember(4)]
    private float defaultMin = 0f;
    [SerializeField]
    [ProtoMember(5)]
    private float defaultMax = 0f;

    // Runtime values

    [ProtoMember(6)]
    private List<Stat> bases = new List<Stat>();
    [ProtoMember(7)]
    private List<Stat> multipliers = new List<Stat>();
    [ProtoMember(8)]
    private List<Stat> addends = new List<Stat>();

    [ProtoMember(9)]
    public Stat min = null;
    [ProtoMember(10)]
    public Stat max = null;

    // Accessor properties

    private float? basis = null;
    public float Basis
    {
        get
        {
            if (!basis.HasValue)
            {
                basis = 0f;

                // Build new list of stats that will contain at most one float stat
                List<Stat> stats = new List<Stat>();
                float floatStat = 0f;

                foreach (Stat stat in bases)
                {
                    basis += stat.Value;

                    if (stat is SimpleStat)
                    {
                        // Compound float stats
                        floatStat += stat.Value;
                    }
                    else
                    {
                        // Leave other stats alone
                        stats.Add(stat);
                    }
                }

                if (floatStat != 0f)
                {
                    // Actually add float stat
                    stats.Add(new SimpleStat(floatStat));
                }

                bases = stats;
            }

            return basis.Value;
        }
    }

    private float? multiplier = null;
    public float Multiplier
    {
        get
        {
            if (!multiplier.HasValue)
            {
                multiplier = 1f;

                // Build new list of stats that will contain at most one float stat
                List<Stat> stats = new List<Stat>();
                float floatStat = 1f;

                foreach (Stat stat in multipliers)
                {
                    multiplier *= stat.Value;

                    if (stat is SimpleStat)
                    {
                        // Compound float stats
                        floatStat *= stat.Value;
                    }
                    else
                    {
                        // Leave other stats alone
                        stats.Add(stat);
                    }
                }

                if (floatStat != 1f)
                {
                    // Actually add float stat
                    stats.Add(new SimpleStat(floatStat));
                }

                multipliers = stats;
            }

            return multiplier.Value;
        }
    }

    private float? addend = null;
    public float Addend
    {
        get
        {
            if (!addend.HasValue)
            {
                addend = 0f;

                // Build new list of stats that will contain at most one float stat
                List<Stat> stats = new List<Stat>();
                float floatStat = 0f;

                foreach (Stat stat in addends)
                {
                    addend += stat.Value;

                    if (stat is SimpleStat)
                    {
                        // Compound float stats
                        floatStat += stat.Value;
                    }
                    else
                    {
                        // Leave other stats alone
                        stats.Add(stat);
                    }
                }

                if (floatStat != 0f)
                {
                    // Actually add float stat
                    stats.Add(new SimpleStat(floatStat));
                }

                addends = stats;
            }

            return addend.Value;
        }
    }

    public float? Min { get { return min?.Value; } }
    public float? Max { get { return max?.Value; } }

    private float? value = null;
    public override float Value
    {
        get
        {
            if (!value.HasValue)
            {
                if (min == null && max == null || min?.Value > max?.Value)
                {
                    // No bounds set or invalid
                    value = Actual;
                }
                else
                {
                    if (min != null)
                    {
                        value = Mathf.Max(min.Value, Actual);
                    }
                    if (max != null)
                    {
                        value = Mathf.Min(min.Value, Actual);
                    }
                }
            }

            return value.Value;
        }
    }

    private float? actual = null;
    public float Actual
    {
        get
        {
            if (!actual.HasValue)
            {
                actual = Basis * Multiplier + Addend;
            }

            return actual.Value;
        }
    }

    public float DefaultValue { get { return defaultMin > defaultMax ? DefaultActual : Mathf.Clamp(DefaultActual, defaultMin, defaultMax); } }
    public float DefaultActual { get { return defaultBasis * defaultMultiplier + defaultAddend; } }

    public float Improvement { get { return Value / DefaultValue; } }
    public float ActualImprovement { get { return Actual / DefaultActual; } }

    /// <summary>
    /// Constructs a <see cref="ComplexStat"/> from the default values set and serialized by unity.
    /// </summary>
    public ComplexStat()
    {
        AddBasis(defaultBasis);
        AddMultiplier(defaultMultiplier);
        AddAddend(defaultAddend);

        min = new SimpleStat(defaultMin);
        max = new SimpleStat(defaultMax);
    }

    public void AddBasis(Stat basis)
    {
        basis.AssertNotNull(nameof(basis));

        bases.Add(basis);

        // Refresh modified property and dependencies
        this.basis = null;
        actual = null;
        value = null;
    }

    public void AddBasis(float basis)
    {
        AddBasis(new SimpleStat(basis));
    }

    public bool RemoveBasis(Stat basis)
    {
        basis.AssertNotNull(nameof(basis));

        bool removed = bases.Remove(basis);
        if (removed)
        {
            // Refresh modified property and dependencies
            this.basis = null;
            actual = null;
            value = null;
        }

        return removed;
    }

    public void RemoveBasis(float basis)
    {
        RemoveBasis(new SimpleStat(basis));
    }

    public void AddMultiplier(Stat multiplier)
    {
        multiplier.AssertNotNull(nameof(multiplier));

        if (multiplier is SimpleStat && multiplier.Value == 0f)
        {
            return;
        }

        multipliers.Add(multiplier);

        // Refresh modified property and dependencies
        this.multiplier = null;
        actual = null;
        value = null;
    }

    public void AddMultiplier(float basis)
    {
        AddMultiplier(new SimpleStat(basis));
    }

    public bool RemoveMultiplier(Stat multiplier)
    {
        multiplier.AssertNotNull(nameof(multiplier));

        bool removed = multipliers.Remove(multiplier);
        if (removed)
        {
            // Refresh modified property and dependencies
            this.multiplier = null;
            actual = null;
            value = null;
        }

        return removed;
    }

    public void RemoveMultiplier(float basis)
    {
        RemoveMultiplier(new SimpleStat(basis));
    }

    public void AddAddend(Stat addend)
    {
        addend.AssertNotNull(nameof(addend));

        addends.Add(addend);

        // Refresh modified property and dependencies
        this.addend = null;
        actual = null;
        value = null;
    }

    public void AddAddend(float basis)
    {
        AddAddend(new SimpleStat(basis));
    }

    public bool RemoveAddend(Stat addend)
    {
        addend.AssertNotNull(nameof(addend));

        bool removed = addends.Remove(addend);
        if (removed)
        {
            // Refresh modified property and dependencies
            this.addend = null;
            actual = null;
            value = null;
        }

        return removed;
    }

    public void RemoveAddend(float basis)
    {
        RemoveAddend(new SimpleStat(basis));
    }

    /// <summary>
    /// Adds <paramref name="num"/> to <see cref="bases"/> of <paramref name="stat"/>.
    /// </summary>
    public static ComplexStat operator +(ComplexStat stat, float num)
    {
        stat.AddBasis(num);
        return stat;
    }

    /// <summary>
    /// Adds <paramref name="num"/> to <see cref="bases"/> of <paramref name="stat"/>.
    /// </summary>
    public static ComplexStat operator +(float num, ComplexStat stat)
    {
        stat.AddBasis(num);
        return stat;
    }

    /// <summary>
    /// Adds negative of <paramref name="num"/> to <see cref="bases"/> of <paramref name="stat"/>.
    /// </summary>
    public static ComplexStat operator -(ComplexStat stat, float num)
    {
        stat.AddBasis(-num);
        return stat;
    }

    /// <summary>
    /// Adds <paramref name="num"/> to <see cref="multipliers"/> of <paramref name="stat"/>.
    /// </summary>
    public static ComplexStat operator *(ComplexStat stat, float num)
    {
        stat.AddMultiplier(num);
        return stat;
    }

    /// <summary>
    /// Adds <paramref name="num"/> to <see cref="multipliers"/> of <paramref name="stat"/>.
    /// </summary>
    public static ComplexStat operator *(float num, ComplexStat stat)
    {
        stat.AddMultiplier(num);
        return stat;
    }

    /// <summary>
    /// Adds inverse <paramref name="num"/> to <see cref="multipliers"/> of <paramref name="stat"/>.
    /// </summary>
    public static ComplexStat operator /(ComplexStat stat, float num)
    {
        stat.AddMultiplier(1f / num);
        return stat;
    }
}