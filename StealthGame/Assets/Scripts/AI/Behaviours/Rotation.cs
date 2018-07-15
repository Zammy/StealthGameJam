using System.Collections;
using UnityEngine;

public static class Rotation
{
    public static IEnumerator Do(IPhysicalEntity physical, float angle, float duration)
    {
        var currentRotation = physical.Rotation;
        var targetRotation = Quaternion.Euler(0, angle, 0) * currentRotation;
        float timeCounter = 0f;
        float speed = duration;
        while (timeCounter < speed)
        {
            physical.Rotation = Quaternion.Lerp(currentRotation, targetRotation, timeCounter / speed);
            timeCounter += Time.deltaTime;
            yield return null;
        }
    }
}