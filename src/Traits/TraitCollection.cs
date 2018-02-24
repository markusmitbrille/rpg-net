using Autrage.LEX.NET;
using System.Collections.ObjectModel;

public class TraitCollection<TKey, TValue> : KeyedCollection<TKey, TValue>
    where TKey : Identity
    where TValue : IUnique<TKey>
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
            IExtendable<TValue> extendable = original as IExtendable<TValue>;
            IDestructible destructible = original as IDestructible;

            extendable?.Extend(item);
            destructible?.Destruct();
        }
        else
        {
            base.InsertItem(index, item);
        }
    }

    protected sealed override void SetItem(int index, TValue item) => InsertItem(index, item);

    protected sealed override void ClearItems() => Bugger.Warning($"Cannot clear {GetType()}!");
}