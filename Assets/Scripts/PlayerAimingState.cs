using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    private readonly int AimingHash = Animator.StringToHash("Aiming");
    private const float AnimationDampTime = 0.1f;


    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.throwingEvent += OnThrow;
    }


    public override void Tick(float deltaTime)
    {
        if (!stateMachine.InputReader.isAiming)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }
        stateMachine.animator.Play(AimingHash);
    }

    public override void Exit()
    {
        stateMachine.InputReader.throwingEvent -= OnThrow;
    }
    private void OnThrow()
    {
        if(!stateMachine.AxeThrown)
             stateMachine.SwitchState(new PlayerThrowingState(stateMachine));
    }

}
