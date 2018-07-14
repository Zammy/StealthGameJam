using UnityEngine;

public static class Vector3Ext
{
    public static Vector3 RandomPosAround(this Vector3 pos, float range)
    {
        var randomDir = RandomDirection();
        return pos + randomDir * range;
    }

    public static Vector3 RandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }
}