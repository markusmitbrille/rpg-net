using Autrage.LEX.NET.Serialization;
using System.Text;
using UnityEngine;

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
    public string Description => ComposeDescription();

    public bool IsFailing => isFailing;
    public bool HasConcluded => hasConcluded;

    public Aura Instantiate(Actor actor, Skill origin, Aura source) => Instantiate(this, actor.transform, source);

    public void Fail() => isFailing = true;

    public void PreventFail() => isFailing = false;

    public bool Is(AuraType type) => this.type.Is(type);

    public bool Is(AuraTags tags) => this.tags.HasFlag(tags);

    public override string ToString() => name;

    private string ComposeDescription()
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

    private void Start()
    {
        // Conclude if no effects were found to avoid dead auras
        if (Effects.Length == 0)
        {
            Conclude();
            return;
        }

        // Conclude if no owner was found to avoid orphaned auras
        if (Owner == null)
        {
            Conclude();
            return;
        }

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
        // Refresh owner and effects once per tick
        owner = null;
        effects = null;

        // Conclude if no effects were found to avoid dead auras
        if (Effects.Length == 0)
        {
            Conclude();
            return;
        }

        // Conclude if no owner was found to avoid orphaned auras
        if (Owner == null)
        {
            Conclude();
            return;
        }

        // Check for failure
        if (isFailing)
        {
            isFailing = false;
            Fail();
            return;
        }

        // Destroy concluded aura when ready
        if (CanDestroy())
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

    private bool CanDestroy()
    {
        if (!hasConcluded)
        {
            return false;
        }

        foreach (Effect effect in Effects)
        {
            if (!effect.CanDestroy())
            {
                return false;
            }
        }

        return true;
    }
}