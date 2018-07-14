using System;
using UnityEngine;

[CreateAssetMenu]
public class WanderingStateData : SMStateData
{
    public float MovementSpeed;
    public float MinDistance;
    public float MaxDistance;
    public float MinWait;
    public float MaxWait;

    public override Type GetStateType()
    {
        return typeof(WanderingState);
    }
}