using System;
using UnityEngine;

public class PlayerActor : Actor
{
    public override Actor Target
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override Vector3 Aim
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override bool IsInCombat
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}
