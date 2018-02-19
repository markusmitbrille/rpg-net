using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
[DisallowMultipleComponent]
public sealed class TransformToken : MonoBehaviour
{
    [DataMember]
    private Vector3 Position
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    [DataMember]
    private Vector3 Rotation
    {
        get { return transform.localEulerAngles; }
        set { transform.localEulerAngles = value; }
    }

    [DataMember]
    private Vector3 Scale
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }
}