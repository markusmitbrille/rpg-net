using Autrage.LEX.NET.Serialization;
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
    private Skill origin;

    [DataMember]
    private Aura source;

    public AuraInfo Info => info;
    public Skill Origin => origin;
    public Aura Source => source;

    public Parent<Actor> Owner { get; private set; }

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

    private void Awake()
    {
        Owner = new Parent<Actor>(this);
    }

    private void Start()
    {
        Owner.Fetch();
        Owner.Instance?.Auras.Fetch();
    }

    private void OnDestroy()
    {
        Owner.Instance?.Auras.Fetch();
    }
}