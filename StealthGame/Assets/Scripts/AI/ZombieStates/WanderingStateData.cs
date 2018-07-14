using System;
using UnityEngine;

[CreateAssetMenu]
public class WanderingStateData : SMStateData
{
    public float MinDistance;
    public float MaxDistance;

    public override Type GetStateType()
    {
        return typeof(WanderingState);
    }
}