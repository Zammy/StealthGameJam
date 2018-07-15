using System.Collections;

public abstract class BaseState : ISMState
{
    protected readonly IEntityContainer _entityContainer;

    protected BaseStateData _data;

    float _noiseLevelAtEnter;

    public BaseState(IEntityContainer entityContainer)
    {
        _entityContainer = entityContainer;
    }

    public virtual void SetData(SMStateData data)
    {
        _data = (BaseStateData)data;
    }

    public virtual void OnStateEnter()
    {
        var agent = _entityContainer.GetEntity<INavMeshAgent>();
        agent.MovementSpeed = _data.MovementSpeed;
        var noise = _entityContainer.GetEntity<NoiseProducerEntity>();
        _noiseLevelAtEnter = noise.NoiseLevel;
    }

    public abstract IEnumerator OnStateExecute(StateMachine stateMachine);

    public virtual void OnStateExit()
    {
        var noise = _entityContainer.GetEntity<NoiseProducerEntity>();
        noise.NoiseLevel = _noiseLevelAtEnter;
    }
}