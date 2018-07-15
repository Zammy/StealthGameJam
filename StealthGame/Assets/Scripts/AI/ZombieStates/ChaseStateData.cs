using System;
using UnityEngine;

[CreateAssetMenu]
public class ChaseStateData : BaseStateData
{
    public override Type GetStateType()
    {
        return typeof(ChaseState);
    }
}