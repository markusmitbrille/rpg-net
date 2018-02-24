using Autrage.LEX.NET.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Serialization/Identity", order = -32000)]
[DataContract]
public class Identity : ScriptableObject
{
    [Auto]
    [SerializeField]
    [DataMember]
    private int id;

    public int ID => id;
}