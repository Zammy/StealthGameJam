using System;
using UnityEngine;

[CreateAssetMenu]
public class SearchLocationStateData : SMStateData
{
    public float MovementSpeed;

    public float AngleLookAround;

    public float LookAroundDuration;

    public override Type GetStateType()
    {
        return typeof(SearchLocationState);
    }
}