using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    void Awake()
    {
        ServiceLocator.Instance.RegisterService(new VisualSystem());
    }

    void Start()
    {
        ServiceLocator.Instance.InitServices();
    }

    void OnDestroy()
    {
        ServiceLocator.Instance.DestroyServices();
    }

    void Update()
    {
        ServiceLocator.Instance.UpdateServices();
    }
}