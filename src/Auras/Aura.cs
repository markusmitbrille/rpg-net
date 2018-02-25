using Autrage.LEX.NET.Extensions;
using Autrage.LEX.NET.Serialization;
using System.Linq;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DisallowMultipleComponent]
[DataContract]
public sealed class Aura : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private AuraInfo info;

    [DataMember]
    private Skill origin;

    [DataMember]
    private Aura source;

    [DataMember]
    public bool Abort { get; set; }

    public AuraInfo Info => info;
    public Skill Origin => origin;
    public Aura Source => source;

    public Parent<Actor> Owner { get; private set; }
    public Family<Inhibitor> Inhibitors { get; private set; }
    public Family<Conductor> Conductors { get; private set; }

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
        Inhibitors = new Family<Inhibitor>(this);
        Conductors = new Family<Conductor>(this);
    }

    private void Start()
    {
        if (Owner.Fetch() == null)
        {
            return;
        }

        if (Owner.Instance.Auras.Lacks(this))
        {
            Owner.Instance.Auras.Fetch();
        }

        if (Inhibitors.All(inhibitor => inhibitor.AllowStart))
        {
            Conductors.ForEach(conductor => conductor.ConductStart());
        }
        else
        {
            Conductors.ForEach(conductor => conductor.ConductTermination());
            Conductors.ForEach(conductor => conductor.ConductConclusion());
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Owner.Instance?.Auras.Contains(this) ?? false)
        {
            Owner.Instance.Auras.Fetch();
        }
    }

    private void Update()
    {
        if (Abort)
        {
            Conductors.ForEach(conductor => conductor.ConductAbortion());
            Conductors.ForEach(conductor => conductor.ConductConclusion());
            Destroy(gameObject);
            return;
        }

        if (Inhibitors.All(inhibitor => inhibitor.AllowUpdate))
        {
            Conductors.ForEach(conductor => conductor.ConductUpdate());
        }
        else
        {
            Conductors.ForEach(conductor => conductor.ConductTermination());
            Conductors.ForEach(conductor => conductor.ConductConclusion());
            Destroy(gameObject);
            return;
        }

        if (Inhibitors.All(inhibitor => inhibitor.AllowCompletion))
        {
            Conductors.ForEach(conductor => conductor.ConductCompletion());
            Conductors.ForEach(conductor => conductor.ConductConclusion());
            Destroy(gameObject);
        }
    }
}