using Autrage.LEX.NET.Extensions;
using System.Collections.Generic;

public enum AuraCategory
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

public static class AuraCategoryExtensions
{
    private static Dictionary<AuraCategory /* child */, AuraCategory /* parent */> immediateParents = new Dictionary<AuraCategory, AuraCategory>()
        {
            { AuraCategory.Aura, AuraCategory.None },
            { AuraCategory.Skill, AuraCategory.Aura },
            { AuraCategory.Attack, AuraCategory.Skill },
            { AuraCategory.Spell, AuraCategory.Skill },
            { AuraCategory.Invocation, AuraCategory.Spell },
            { AuraCategory.Enchantment, AuraCategory.Spell },
            { AuraCategory.Blessing, AuraCategory.Enchantment },
            { AuraCategory.Curse, AuraCategory.Enchantment },
            { AuraCategory.Song, AuraCategory.Skill },
            { AuraCategory.Verse, AuraCategory.Song },
            { AuraCategory.Refrain, AuraCategory.Song },
            { AuraCategory.Bridge, AuraCategory.Song },
            { AuraCategory.Finale, AuraCategory.Song },
        };

    private static Dictionary<AuraCategory /* child */, List<AuraCategory> /* parents */> allParents = new Dictionary<AuraCategory, List<AuraCategory>>();

    static AuraCategoryExtensions()
    {
        // Build all parents
        foreach (AuraCategory root in immediateParents.Keys)
        {
            allParents[root] = new List<AuraCategory>();
            for (AuraCategory parent = immediateParents.GetValueOrDefault(root); parent != AuraCategory.None; parent = immediateParents.GetValueOrDefault(parent))
            {
                allParents[root].Add(parent);
            }
        }
    }

    public static bool Is(this AuraCategory category, AuraCategory other)
    {
        return category == other || allParents[category].Contains(other);
    }
}