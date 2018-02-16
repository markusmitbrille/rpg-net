using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[DisallowMultipleComponent]
public class TransformToken : DataDrivenBehaviour
{
    [ProtoContract(Name = "Vector3Token")]
    struct Vector3Token
    {
        [ProtoMember(1)]
        float x;
        [ProtoMember(2)]
        float y;
        [ProtoMember(3)]
        float z;

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

    [DataMember(1)]
    Vector3Token Position
    {
        get { return new Vector3Token(transform.localPosition); }
        set { transform.localPosition = value.ToVector3(); }
    }

    [DataMember(2)]
    Vector3Token Rotation
    {
        get { return new Vector3Token(transform.localEulerAngles); }
        set { transform.localEulerAngles = value.ToVector3(); }
    }

    [DataMember(3)]
    Vector3Token Scale
    {
        get { return new Vector3Token(transform.localScale); }
        set { transform.localScale = value.ToVector3(); }
    }
}
