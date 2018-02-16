using Autrage.LEX.NET.Extensions;
using System.Collections.Generic;

public enum DamageType
{
    None,
    Damage,
    Physical,
    Blunt,
    Piercing,
    Cutting,
    Elemental,
    Heat,
    Frost,
    Corrosive,
    Galvanic,
    Psychological,
    Trauma,
    Chaos,
    Fatigue,
}

public static class DamageTypeExtensions
{
    private static Dictionary<DamageType /* child */, DamageType /* parent */> immediateParents = new Dictionary<DamageType, DamageType>()
        {
            { DamageType.Damage, DamageType.None },
            { DamageType.Physical, DamageType.Damage },
            { DamageType.Blunt, DamageType.Physical },
            { DamageType.Piercing, DamageType.Physical },
            { DamageType.Cutting, DamageType.Physical },
            { DamageType.Elemental, DamageType.Damage },
            { DamageType.Heat, DamageType.Elemental },
            { DamageType.Frost, DamageType.Elemental },
            { DamageType.Corrosive, DamageType.Elemental },
            { DamageType.Galvanic, DamageType.Elemental },
            { DamageType.Psychological, DamageType.Damage },
            { DamageType.Trauma, DamageType.Psychological },
            { DamageType.Chaos, DamageType.Psychological },
            { DamageType.Fatigue, DamageType.Psychological },
        };

    private static Dictionary<DamageType /* child */, List<DamageType> /* parents */> allParents = new Dictionary<DamageType, List<DamageType>>();

    static DamageTypeExtensions()
    {
        // Build all parents
        foreach (DamageType root in immediateParents.Keys)
        {
            allParents[root] = new List<DamageType>();
            for (DamageType parent = immediateParents.GetValueOrDefault(root); parent != DamageType.None; parent = immediateParents.GetValueOrDefault(parent))
            {
                allParents[root].Add(parent);
            }
        }
    }

    public static bool Is(this DamageType type, DamageType other)
    {
        return type == other || allParents[type].Contains(other);
    }
}