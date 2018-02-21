using Autrage.LEX.NET.Extensions;
using System.Collections.Generic;

public enum AuraCategory
{
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
            List<AuraCategory> parents = new List<AuraCategory>() { default(AuraCategory) };
            for (AuraCategory parent = immediateParents.GetValueOrDefault(root); parent != default(AuraCategory) && !parents.Contains(parent); parent = immediateParents.GetValueOrDefault(parent))
            {
                parents.Add(parent);
            }

            allParents[root] = parents;
        }
    }

    public static bool Is(this AuraCategory category, AuraCategory other)
    {
        return category == other || allParents[category].Contains(other);
    }
}