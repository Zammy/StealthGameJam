using System;
using UnityEngine;

[CreateAssetMenu]
public class SearchLocationStateData : BaseStateData
{
    public float AngleLookAround;
    public float LookAroundDuration;

    public override Type GetStateType()
    {
        return typeof(SearchLocationState);
    }
}