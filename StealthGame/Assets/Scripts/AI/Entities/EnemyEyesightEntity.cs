using UnityEngine;

public class EnemyEyesightEntity : EyesightEntity
{
    public Vector3? PlayerSpottedPosition { get; set; }
    public float PlayerTimeBeingSpotted { get; set; }

    public EnemyEyesightEntity(float fov, float distance, Transform headTransform)
        : base(fov, distance, headTransform)
    {
    }
}