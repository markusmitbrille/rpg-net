using Autrage.LEX.NET.Serialization;
using System.Linq;
using System.Text;
using UnityEngine;
using static Autrage.LEX.NET.Bugger;

[DisallowMultipleComponent]
[DataContract]
public class Aura : MonoBehaviour
{
    private const string DescriptionSeperator = "---";

    [SerializeField]
    [DataMember]
    private AuraInfo info;

    [DataMember]
    private Skill origin;

    [DataMember]
    private Aura source;

    private Actor owner;
    private Effect[] effects;

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

    public Skill Origin => origin;
    public Aura Source => source;

    public Actor Owner => owner ?? (owner = GetComponentInParent<Actor>());
    public Effect[] Effects => effects ?? (effects = GetComponents<Effect>());

    public bool IsFailing => isFailing;
    public bool IsApplied => isApplied;
    public bool IsCompleted => isCompleted;
    public bool IsFailed => isFailed;
    public bool IsTerminated => isTerminated;
    public bool IsConcluded => isConcluded;
    public bool CanDestroy => IsConcluded && Effects.All(effect => effect.CanDestroy());

    public void Fail() => isFailing = true;

    public void PreventFail() => isFailing = false;

    public bool Is(AuraCategory category) => info.Category.Is(category);

    public bool Is(AuraTags tags) => info.Tags.HasFlag(tags);

    public override string ToString() => info.Title;

    public string GetDescription()
    {
        // Create string builder for performant string concatenation
        StringBuilder description = new StringBuilder(info.Description);

        foreach (Effect effect in Effects)
        {
            // Append the seperator with blank lines before and after
            description.AppendLine().AppendLine(DescriptionSeperator).AppendLine();

            // Append the effect's description
            description.Append(effect.Description);
        }

        if (!string.IsNullOrEmpty(info.Flavour) && !string.IsNullOrWhiteSpace(info.Flavour))
        {
            // Append the seperator with blank lines before and after
            description.AppendLine().AppendLine(DescriptionSeperator).AppendLine();

            // Append the ability's flavour text
            description.AppendLine(info.Flavour);
        }

        return description.ToString();
    }

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
        // Self-destruct if no owner was found to avoid orphaned auras
        if (Owner == null)
        {
            Error($"Could not get owner of {this}!");
            Destroy(this);
            return;
        }
        // Self-destruct if no effects were found to avoid dead auras
        if (Effects.Length == 0)
        {
            Warning($"No effects found on {this}!");
            Destroy(this);
            return;
        }

        Apply();
    }

    private void Apply()
    {
        if (isApplied)
        {
            return;
        }

        isApplied = true;

        // Pre-application stage
        Effect.StageResults preApplicationResults = Effects.Aggregate(Effect.StageResults.None, (res, effect) => res | effect.OnPreApplication());
        if (preApplicationResults.HasFlag(Effect.StageResults.Failed))
        {
            Failure();
            return;
        }
        if (preApplicationResults.HasFlag(Effect.StageResults.Terminated))
        {
            Terminate();
            return;
        }

        // Application stage
        Effect.StageResults applicationResults = Effects.Aggregate(Effect.StageResults.None, (res, effect) => res | effect.OnApplication());
        if (applicationResults.HasFlag(Effect.StageResults.Failed))
        {
            Failure();
            return;
        }
        if (applicationResults == Effect.StageResults.CanComplete)
        {
            Complete();
            return;
        }
    }

    private void Update()
    {
        // Check for failure
        if (isFailing)
        {
            isFailing = false;
            Fail();
            return;
        }

        // Destroy concluded aura when ready
        if (CanDestroy)
        {
            Destroy(gameObject);
            return;
        }

        // Pre-update stage
        Effect.StageResults preUpdateResults = Effects.Aggregate(Effect.StageResults.None, (res, effect) => res | effect.OnPreUpdate());
        if (preUpdateResults.HasFlag(Effect.StageResults.Failed))
        {
            Failure();
            return;
        }
        if (preUpdateResults.HasFlag(Effect.StageResults.Terminated))
        {
            Terminate();
            return;
        }

        // Update stage
        Effect.StageResults updateResults = Effects.Aggregate(Effect.StageResults.None, (res, effect) => res | effect.OnUpdate());
        if (updateResults.HasFlag(Effect.StageResults.Failed))
        {
            Failure();
            return;
        }
        if (updateResults == Effect.StageResults.CanComplete)
        {
            Complete();
            return;
        }
    }

    private void Complete()
    {
        if (isCompleted)
        {
            return;
        }

        isCompleted = true;

        foreach (Effect effect in Effects)
        {
            effect.OnCompletion();
        }

        Conclude();
    }

    private void Terminate()
    {
        if (isTerminated)
        {
            return;
        }

        isTerminated = true;

        foreach (Effect effect in Effects)
        {
            effect.OnTermination();
        }

        Conclude();
    }

    private void Failure()
    {
        if (isFailed)
        {
            return;
        }

        isFailed = true;

        foreach (Effect effect in Effects)
        {
            effect.OnFailure();
        }

        Conclude();
    }

    private void Conclude()
    {
        if (isConcluded)
        {
            return;
        }

        isConcluded = true;

        foreach (Effect effect in Effects)
        {
            effect.OnConclusion();
        }
    }
}