using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowingState : PlayerBaseState
{
    private readonly int throwHash = Animator.StringToHash("Throwing");

    public PlayerThrowingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.animator.Play(throwHash);
    }
    public override void Tick(float deltaTime)
    {
        Debug.Log("is Throwing State");
        //stateMachine.SwitchState(new PlayerAimingState(stateMachine));

    }

    public override void Exit()
    {
        Debug.Log("Exited Throwing State");
    }

}
