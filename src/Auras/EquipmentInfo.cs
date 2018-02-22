using Autrage.LEX.NET.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Serialization/Equipment Info")]
[DataContract]
public class EquipmentInfo : Info
{
    [SerializeField]
    [DataMember]
    private EquipmentCategory category;

    [SerializeField]
    [DataMember]
    private Aura equip;

    [SerializeField]
    [DataMember]
    private Aura unequip;

    public EquipmentCategory Category => category;
    public Aura Equip => equip;
    public Aura Unequip => unequip;
}