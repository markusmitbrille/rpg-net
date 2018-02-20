using Autrage.LEX.NET.Serialization;

[DataContract]
public class ActorSelector : ColliderSelector<Actor>
{
    public override string ToString()
    {
        string result = base.ToString();
        return string.IsNullOrEmpty(result) ? "no one" : result;
    }
}

