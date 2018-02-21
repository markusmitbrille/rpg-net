using Autrage.LEX.NET.Extensions;
using System.Collections.Generic;

public enum EquipmentCategory
{
    Equipment,
    Apparel,
    Armour,
    Light,
    Mail,
    Plate,
    Weapon,
    Blade,
    Sword,
    Axe,
    Dagger,
    Blunt,
    Mace,
    Hammer,
    Polearm,
    Staff,
    Magic,
    Wand,
    Idol,
    Focus
}

public static class EquipmentCategoryExtensions
{
    private static Dictionary<EquipmentCategory /* child */, EquipmentCategory /* parent */> immediateParents = new Dictionary<EquipmentCategory, EquipmentCategory>()
        {
            { EquipmentCategory.Armour, EquipmentCategory.Apparel },
        };

    private static Dictionary<EquipmentCategory /* child */, List<EquipmentCategory> /* parents */> allParents = new Dictionary<EquipmentCategory, List<EquipmentCategory>>();

    static EquipmentCategoryExtensions()
    {
        // Build all parents
        foreach (EquipmentCategory root in immediateParents.Keys)
        {
            List<EquipmentCategory> parents = new List<EquipmentCategory>() { default(EquipmentCategory) };
            for (EquipmentCategory parent = immediateParents.GetValueOrDefault(root); parent != default(EquipmentCategory) && !parents.Contains(parent); parent = immediateParents.GetValueOrDefault(parent))
            {
                parents.Add(parent);
            }

            allParents[root] = parents;
        }
    }

    public static bool Is(this EquipmentCategory category, EquipmentCategory other)
    {
        return category == other || allParents[category].Contains(other);
    }
}