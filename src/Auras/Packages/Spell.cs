using UnityObject = UnityEngine.Object;

public class Spell : Package
{
    public Aura Prefab { get; set; }

    public override bool IsValid => base.IsValid && Prefab != null;

    public override Report Unwrap()
    {
        UnityObject.Instantiate(Prefab, Receiver.transform);
        return new SpellReport(Source, Sender, Receiver, Prefab);
    }
}
