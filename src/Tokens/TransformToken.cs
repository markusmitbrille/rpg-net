using Autrage.LEX.NET.Serialization;
using UnityEngine;

[DataContract]
[DisallowMultipleComponent]
public class TransformToken : MonoBehaviour
{
    [DataMember]
    private Vector3Token Position
    {
        get { return new Vector3Token(transform.localPosition); }
        set { transform.localPosition = value.ToVector3(); }
    }

    [DataMember]
    private Vector3Token Rotation
    {
        get { return new Vector3Token(transform.localEulerAngles); }
        set { transform.localEulerAngles = value.ToVector3(); }
    }

    [DataMember]
    private Vector3Token Scale
    {
        get { return new Vector3Token(transform.localScale); }
        set { transform.localScale = value.ToVector3(); }
    }

    [DataContract]
    private struct Vector3Token
    {
        [DataMember]
        private float x;

        [DataMember]
        private float y;

        [DataMember]
        private float z;

        public Vector3Token(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        public Vector3 ToVector3()
        {
            Vector3 result;
            result.x = x;
            result.y = y;
            result.z = z;
            return result;
        }
    }
}