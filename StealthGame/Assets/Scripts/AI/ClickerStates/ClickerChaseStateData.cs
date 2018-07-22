using System;
using UnityEngine;

[CreateAssetMenu]
public class ClickerChaseStateData : BaseStateData
{
    public override Type GetStateType()
    {
        return typeof(ClickerChaseState);
    }
}