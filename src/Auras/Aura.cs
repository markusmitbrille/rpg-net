using Autrage.LEX.NET;
using Autrage.LEX.NET.Serialization;
using System.Linq;
using System.Text;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DisallowMultipleComponent]
[DataContract]
public sealed class Aura : MonoBehaviour
{
    private const string DescriptionSeperator = "---";

    [SerializeField]
    [DataMember]
    private AuraInfo id;

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

    public AuraInfo ID => id;
    public Actor Owner { get; private set; }

    public Skill Origin => origin;
    public Aura Source => source;

    public bool Is(AuraCategory category) => id.Category.Is(category);

    public bool Is(AuraTags tags) => id.Tags.HasFlag(tags);

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
}