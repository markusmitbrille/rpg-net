using Autrage.LEX.NET.Extensions;
using System.Collections.Generic;

public enum DamageCategory
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

public static class DamageCategoryExtensions
{
    private static Dictionary<DamageCategory /* child */, DamageCategory /* parent */> immediateParents = new Dictionary<DamageCategory, DamageCategory>()
        {
            { DamageCategory.Damage, DamageCategory.None },
            { DamageCategory.Physical, DamageCategory.Damage },
            { DamageCategory.Blunt, DamageCategory.Physical },
            { DamageCategory.Piercing, DamageCategory.Physical },
            { DamageCategory.Cutting, DamageCategory.Physical },
            { DamageCategory.Elemental, DamageCategory.Damage },
            { DamageCategory.Heat, DamageCategory.Elemental },
            { DamageCategory.Frost, DamageCategory.Elemental },
            { DamageCategory.Corrosive, DamageCategory.Elemental },
            { DamageCategory.Galvanic, DamageCategory.Elemental },
            { DamageCategory.Psychological, DamageCategory.Damage },
            { DamageCategory.Trauma, DamageCategory.Psychological },
            { DamageCategory.Chaos, DamageCategory.Psychological },
            { DamageCategory.Fatigue, DamageCategory.Psychological },
        };

    private static Dictionary<DamageCategory /* child */, List<DamageCategory> /* parents */> allParents = new Dictionary<DamageCategory, List<DamageCategory>>();

    static DamageCategoryExtensions()
    {
        // Build all parents
        foreach (DamageCategory root in immediateParents.Keys)
        {
            allParents[root] = new List<DamageCategory>();
            for (DamageCategory parent = immediateParents.GetValueOrDefault(root); parent != DamageCategory.None; parent = immediateParents.GetValueOrDefault(parent))
            {
                allParents[root].Add(parent);
            }
        }
    }

    public static bool Is(this DamageCategory category, DamageCategory other)
    {
        return category == other || allParents[category].Contains(other);
    }
}