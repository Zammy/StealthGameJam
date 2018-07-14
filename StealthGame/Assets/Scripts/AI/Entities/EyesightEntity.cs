using System.Collections.Generic;
using UnityEngine;

public class EyesightEntity : IEntity
{
    readonly Transform _headTransform;

    public float Distance { get; private set; }
    public float FOV { get; private set; }
    public Vector3 EyeDirection { get { return _headTransform.forward; } }
    public Vector3 HeadPosition { get { return _headTransform.position; } }


    public EyesightEntity(float fov, float distance, Transform headTransform)
    {
        Distance = distance;
        FOV = fov;
        _headTransform = headTransform;
    }
}