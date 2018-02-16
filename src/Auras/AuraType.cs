using Autrage.LEX.NET.Extensions;
using System.Collections.Generic;

public enum AuraType
{
    None,
    Aura,
    Skill,
    Attack,
    Spell,
    Invocation,
    Enchantment,
    Blessing,
    Curse,
    Song,
    Verse,
    Refrain,
    Bridge,
    Finale,
}

public static class AuraTypeExtensions
{
    private static Dictionary<AuraType /* child */, AuraType /* parent */> immediateParents = new Dictionary<AuraType, AuraType>()
        {
            { AuraType.Aura, AuraType.None },
            { AuraType.Skill, AuraType.Aura },
            { AuraType.Attack, AuraType.Skill },
            { AuraType.Spell, AuraType.Skill },
            { AuraType.Invocation, AuraType.Spell },
            { AuraType.Enchantment, AuraType.Spell },
            { AuraType.Blessing, AuraType.Enchantment },
            { AuraType.Curse, AuraType.Enchantment },
            { AuraType.Song, AuraType.Skill },
            { AuraType.Verse, AuraType.Song },
            { AuraType.Refrain, AuraType.Song },
            { AuraType.Bridge, AuraType.Song },
            { AuraType.Finale, AuraType.Song },
        };

    private static Dictionary<AuraType /* child */, List<AuraType> /* parents */> allParents = new Dictionary<AuraType, List<AuraType>>();

    static AuraTypeExtensions()
    {
        // Build all parents
        foreach (AuraType root in immediateParents.Keys)
        {
            allParents[root] = new List<AuraType>();
            for (AuraType parent = immediateParents.GetValueOrDefault(root); parent != AuraType.None; parent = immediateParents.GetValueOrDefault(parent))
            {
                allParents[root].Add(parent);
            }
        }
    }

    public static bool Is(this AuraType type, AuraType other)
    {
        return type == other || allParents[type].Contains(other);
    }
}