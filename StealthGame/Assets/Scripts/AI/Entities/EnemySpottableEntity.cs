using UnityEngine;

public class EnemySpottableEntity : SpottableEntity
{
    GameObject _visual;

    public override bool Spotted
    {
        get
        {
            return base.Spotted;
        }
        set
        {
            base.Spotted = value;
            _visual.SetActive(value);
        }
    }

    public EnemySpottableEntity(GameObject visual, Collider collider)
        : base(collider)
    {
        _visual = visual;
    }
}