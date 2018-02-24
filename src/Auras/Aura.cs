using Autrage.LEX.NET.Extensions;
using Autrage.LEX.NET.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DisallowMultipleComponent]
[DataContract]
public sealed class Aura : MonoBehaviour
{
    private const string DescriptionSeperator = "---";

    [SerializeField]
    [DataMember]
    private AuraInfo info;

    [DataMember]
    private Actor actor;

    [DataMember]
    private Skill origin;

    [DataMember]
    private Aura source;

    [DataMember]
    private bool isFailing;

    [DataMember]
    private bool isApplied;

    [DataMember]
    private bool isCompleted;

    [DataMember]
    private bool isFailed;

    [DataMember]
    private bool isTerminated;

    [DataMember]
    private bool isConcluded;

    public AuraInfo Info => info;
    public Actor Owner { get; private set; }

    public Skill Origin => origin;
    public Aura Source => source;

    public bool Is(AuraCategory category) => info.Category.Is(category);

    public bool Is(AuraTags tags) => info.Tags.HasFlag(tags);

    public Aura Create(Actor actor, Skill origin, Aura source)
    {
        GameObject auraGameObject = Instantiate(gameObject, actor.transform);
        Aura aura = auraGameObject.GetComponents<Aura>().SingleOrDefault();
        if (aura == null)
        {
            Error($"Multiple instances of {nameof(Aura)} found on {auraGameObject}!");
            Destroy(auraGameObject);
            return null;
        }

        aura.origin = origin;
        aura.source = source;
        return aura;
    }

    private void Start()
    {
        Owner = GetComponentInParent<Actor>();
        if (Owner == null)
        {
            Error($"Could not get {nameof(Owner)} of {GetType()} {this}!");
            Destroy(this);
            return;
        }
    }

    private void OnDestroy()
    {
    }

    public class Collection : ICollection<Aura>, IEnumerable<Aura>, IEnumerable
    {
        private Dictionary<AuraInfo, HashSet<Aura>> dictionary = new Dictionary<AuraInfo, HashSet<Aura>>();

        public int Count => dictionary.Values.Sum(set => set.Count);
        bool ICollection<Aura>.IsReadOnly => false;
        public HashSet<Aura> this[AuraInfo info] => dictionary.GetValueOrDefault(info);

        public bool Add(Aura aura)
        {
            HashSet<Aura> set = dictionary.GetValueOrDefault(aura.info);
            if (set == null)
            {
                set = new HashSet<Aura>(new ReferenceComparer());
                dictionary[aura.info] = set;
            }

            return set.Add(aura);
        }

        public void Clear()
        {
            foreach (HashSet<Aura> set in dictionary.Values)
            {
                foreach (Aura aura in set)
                {
                    Destroy(aura.gameObject);
                }

                set.Clear();
            }

            dictionary.Clear();
        }

        public bool Contains(Aura aura)
        {
            HashSet<Aura> set = dictionary.GetValueOrDefault(aura.info);
            if (set == null)
            {
                return false;
            }

            return set.Contains(aura);
        }

        public void CopyTo(Aura[] array, int arrayIndex)
        {
            foreach (HashSet<Aura> set in dictionary.Values)
            {
                set.CopyTo(array, arrayIndex);
                arrayIndex += set.Count;
            }
        }

        public bool Remove(Aura aura)
        {
            HashSet<Aura> set = dictionary.GetValueOrDefault(aura.info);
            if (set == null)
            {
                return false;
            }

            return set.Remove(aura);
        }

        void ICollection<Aura>.Add(Aura aura) => Add(aura);

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (HashSet<Aura> set in dictionary.Values)
            {
                foreach (Aura aura in set)
                {
                    yield return aura;
                }
            }
        }

        IEnumerator<Aura> IEnumerable<Aura>.GetEnumerator()
        {
            foreach (HashSet<Aura> set in dictionary.Values)
            {
                foreach (Aura aura in set)
                {
                    yield return aura;
                }
            }
        }
    }
}