using Zenject;
using UnityEngine;

public class MenuBootstrap : MonoBehaviour
{
    private StateMachine _stateMachine;

    [Inject]
    private void Construct(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    
    private void Start()
    {
        _stateMachine.ChangeState<MenuState>();
    }
}
