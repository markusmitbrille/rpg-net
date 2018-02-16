using Autrage.LEX.NET.Serialization;
using System.Text;
using UnityEngine;

/// <summary>
/// An <see cref="Aura"/> is an aggregative behaviour that marshals <see cref="Effect"/>s
/// and goes through a complex lifecycle doing so. It considers all <see cref="Effect"/>s
/// on its <see cref="GameObject"/> as its own.
/// </summary>
[DisallowMultipleComponent]
[DataContract]
public class Aura : MonoBehaviour
{
    private const string DescriptionSeperator = "---";

    [BitMask(typeof(AuraTags))]
    [SerializeField]
    [DataMember]
    private AuraTags tags = AuraTags.None;

    [SerializeField]
    [DataMember]
    private AuraType type = AuraType.None;

    [Header("On Conclusion")]
    [Tooltip("Destroys the gameObject on conclusion.")]
    [SerializeField]
    [DataMember]
    private bool destroyGameObject = true;

    [Tooltip("Destroys the aura monobehaviour on conclusion.")]
    [SerializeField]
    [DataMember]
    private bool destroySelf = false;

    [Tooltip("Destroys all effect monobehavious on conclusion.")]
    [SerializeField]
    [DataMember]
    private bool destroyEffects = false;

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
    private bool failNextTick;

    private Actor owner = null;
    private Effect[] effects = null;

    public Actor Owner
    {
        get
        {
            if (owner == null)
            {
                owner = GetComponentInParent<Actor>();
            }

            return owner;
        }
    }

    public Effect[] Effects
    {
        get
        {
            if (effects == null)
            {
                effects = GetComponents<Effect>();
            }

            return effects;
        }
    }

    public string Description
    {
        get
        {
            // Create string builder with summary for performant string concatenation
            StringBuilder description = new StringBuilder(summary);

            foreach (Effect effect in Effects)
            {
                // Append the seperator with blank lines before and after
                description.AppendLine().AppendLine(DescriptionSeperator).AppendLine();

                // Append the effect's description
                effect.AppendDescription(description);
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

    public void Fail()
    {
        failNextTick = true;
    }

    public void PreventFail()
    {
        failNextTick = false;
    }

    public bool Is(AuraType type)
    {
        return this.type.Is(type);
    }

    public bool Is(AuraTags tags)
    {
        return this.tags.HasFlag(tags);
    }

    private void Start()
    {
        // Conclude if no effects were found to avoid dead auras
        if (Effects.Length == 0)
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
        if (applicationResults.HasFlag(Effect.StageResults.Completed))
        {
            Complete();
            return;
        }
    }

    private void Update()
    {
        // Conclude if no effects were found to avoid dead auras
        if (Effects.Length == 0)
        {
            Conclude();
            return;
        }

        // Refresh owner and effects once per tick
        owner = null;
        effects = null;

        // Check for failure
        if (failNextTick)
        {
            failNextTick = false;
            Fail();
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
        if (updateResults.HasFlag(Effect.StageResults.Completed))
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

        if (destroyEffects)
        {
            foreach (Effect effect in Effects)
            {
                Destroy(effect);
            }
        }

        if (destroySelf)
        {
            Destroy(this);
        }

        if (destroyGameObject)
        {
            Destroy(gameObject);
        }
    }
}