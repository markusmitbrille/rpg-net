using Autrage.LEX.NET;
using System.Collections.ObjectModel;

public class TraitCollection<TKey, TValue> : KeyedCollection<TKey, TValue>
    where TKey : Identity
    where TValue : IUnique<TKey>, IExtendable<TValue>, IDestructible
{
    public TraitCollection() : base(new IdentityEqualityComparer<TKey>())
    {
    }

    protected override TKey GetKeyForItem(TValue item) => item.ID;

    protected sealed override void InsertItem(int index, TValue item)
    {
        if (item == null)
        {
            Bugger.Warning($"Tried to insert null!");
            return;
        }

        TKey key = GetKeyForItem(item);
        if (Contains(key))
        {
            TValue original = this[key];
            original.Extend(item);
            item.Destruct();
        }
        else
        {
            base.InsertItem(index, item);
        }
    }

    protected sealed override void SetItem(int index, TValue item) => InsertItem(index, item);

    protected sealed override void ClearItems() => Bugger.Warning($"Cannot clear {GetType()}!");
}