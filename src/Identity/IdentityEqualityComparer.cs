using System.Collections.Generic;

public class IdentityEqualityComparer<T> : IEqualityComparer<T> where T : Identity
{
    public bool Equals(T x, T y) => x?.ID == y?.ID;

    public int GetHashCode(T obj) => obj?.ID ?? 0;
}
