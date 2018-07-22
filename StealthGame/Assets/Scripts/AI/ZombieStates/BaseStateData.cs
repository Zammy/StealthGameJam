using UnityEngine.Serialization;

public abstract class BaseStateData : SMStateData
{
    public float MovementSpeed;
    [FormerlySerializedAsAttribute("ExtraNoiseWhenWalking")]
    public float WalkExtraNoise;
    public float RoarExtraNoise;

}