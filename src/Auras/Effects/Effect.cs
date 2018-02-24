using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public abstract class Effect : MonoBehaviour
{
    [SerializeField]
    [DataMember]
    private EffectInfo info;

    public EffectInfo Info => info;
    public Parent<Aura> Aura { get; private set; }
    public Parent<Actor> Owner { get; private set; }

    private void Awake()
    {
        Aura = new Parent<Aura>(this);
        Owner = new Parent<Actor>(this);
    }

    private void Start()
    {
        Aura.Fetch();
        Owner.Fetch();
    }
}