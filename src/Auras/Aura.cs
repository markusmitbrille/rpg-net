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

    [DataMember]
    private Skill origin;

    [DataMember]
    private Aura source;

    private Actor owner;
    private Effect[] effects;

    [SerializeField]
    [ReadOnly]
    [DataMember]
    private int id;

    [BitMask(typeof(AuraTags))]
    [SerializeField]
    [DataMember]
    private AuraTags tags;

    [SerializeField]
    [DataMember]
    private AuraType type;

    [Header("Description")]
    [TextArea]
    [SerializeField]
    [DataMember]
    private string summary = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string flavour = "";

    [DataMember]
    private bool isFailing;

    [DataMember]
    private bool hasConcluded;

    public Skill Origin => origin;
    public Aura Source => source;

    public Actor Owner => owner ?? (owner = GetComponentInParent<Actor>());
    public Effect[] Effects => effects ?? (effects = GetComponents<Effect>());

    public int ID => id;
    public AuraTags Tags => tags;
    public AuraType Type => type;

    public string Name => name;
    public string Summary => summary;
    public string Flavour => flavour;

    public string Description
    {
        get
        {
            // Create string builder for performant string concatenation
            StringBuilder description = new StringBuilder(summary);

            foreach (Effect effect in Effects)
            {
                // Append the seperator with blank lines before and after
                description.AppendLine().AppendLine(DescriptionSeperator).AppendLine();

                // Append the effect's description
                description.Append(effect.Description);
            }

            if (!string.IsNullOrEmpty(flavour) && !string.IsNullOrWhiteSpace(flavour))
            {
                // Append the seperator with blank lines before and after
                description.AppendLine().AppendLine(DescriptionSeperator).AppendLine();

                // Append the ability's flavour text
                description.AppendLine(flavour);
            }

            return description.ToString();
        }
    }

    public bool IsFailing => isFailing;
    public bool HasConcluded => hasConcluded;
    public bool CanDestroy => HasConcluded && Effects.All(effect => effect.CanDestroy());

    public void Fail() => isFailing = true;

    public void PreventFail() => isFailing = false;

    public bool Is(AuraType type) => this.type.Is(type);

    public bool Is(AuraTags tags) => this.tags.HasFlag(tags);

    public override string ToString() => name;

    public Aura Instantiate(Actor actor, Skill origin, Aura source)
    {
        Aura aura = Instantiate(this, actor.transform, source);
        aura.Apply();
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
    }

    private void Apply()
    {
        // Pre-application stage
        Effect.StageResults preApplicationResults = Effect.StageResults.None;
        foreach (Effect effect in Effects)
        {
            preApplicationResults |= effect.OnPreApplication();
        }

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
        Effect.StageResults applicationResults = Effect.StageResults.None;
        foreach (Effect effect in Effects)
        {
            applicationResults |= effect.OnApplication();
        }

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
        Effect.StageResults preUpdateResults = Effect.StageResults.None;
        foreach (Effect effect in Effects)
        {
            preUpdateResults |= effect.OnPreUpdate();
        }

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
        Effect.StageResults updateResults = Effect.StageResults.None;
        foreach (Effect effect in Effects)
        {
            updateResults |= effect.OnUpdate();
        }

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
        foreach (Effect effect in Effects)
        {
            effect.OnCompletion();
        }
    }

    private void Terminate()
    {
        foreach (Effect effect in Effects)
        {
            effect.OnTermination();
        }
    }

    private void Failure()
    {
        foreach (Effect effect in Effects)
        {
            effect.OnFailure();
        }
    }

    private void Conclude()
    {
        foreach (Effect effect in Effects)
        {
            effect.OnConclusion();
        }

        hasConcluded = true;
    }
}