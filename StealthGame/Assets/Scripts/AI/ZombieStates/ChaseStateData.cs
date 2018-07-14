using System;
using UnityEngine;

[CreateAssetMenu]
public class ChaseStateData : SMStateData
{
    public float MovementSpeed;

    public override Type GetStateType()
    {
        return typeof(ChaseState);
    }
}