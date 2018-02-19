using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
public class LuaEffect : Effect
{
    private const string defaultChunkName = "Lua Effect Script";

    [TextArea]
    [SerializeField]
    [DataMember]
    private string description = "";

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

    public override string Description => description;

    public override StageResults OnPreApplication()
    {
        object[] results = Monoton<LuaSandbox>.Instance.DoString(onPreApplication ?? "", chunkName);
        return results.Length > 0 ? results[0] as StageResults? ?? StageResults.None : StageResults.None;
    }

    public override StageResults OnPreUpdate()
    {
        object[] results = Monoton<LuaSandbox>.Instance.DoString(onPreUpdate ?? "", chunkName);
        return results.Length > 0 ? results[0] as StageResults? ?? StageResults.None : StageResults.None;
    }

    public override StageResults OnApplication()
    {
        object[] results = Monoton<LuaSandbox>.Instance.DoString(onApplication ?? "", chunkName);
        return results.Length > 0 ? results[0] as StageResults? ?? StageResults.None : StageResults.None;
    }

    public override StageResults OnUpdate()
    {
        object[] results = Monoton<LuaSandbox>.Instance.DoString(onUpdate ?? "", chunkName);
        return results.Length > 0 ? results[0] as StageResults? ?? StageResults.None : StageResults.None;
    }

    public override void OnCompletion() => Monoton<LuaSandbox>.Instance.DoString(onCompletion ?? "", chunkName);

    public override void OnTermination() => Monoton<LuaSandbox>.Instance.DoString(onTermination ?? "", chunkName);

    public override void OnFailure() => Monoton<LuaSandbox>.Instance.DoString(onFailure ?? "", chunkName);

    public override void OnConclusion() => Monoton<LuaSandbox>.Instance.DoString(onConclusion ?? "", chunkName);
}