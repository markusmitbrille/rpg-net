using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public class LuaEffect : Effect
{
    private const string defaultChunkName = "Lua Effect Script";

    [SerializeField]
    [DataMember]
    private string chunkName = defaultChunkName;

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onPreApplication = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onApplication = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onPreUpdate = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onUpdate = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onCompletion = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onTermination = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onFailure = "";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string onConclusion = "";
}