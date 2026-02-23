using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimingState : PlayerBaseState
{
    private readonly int AimingHash = Animator.StringToHash("Aiming");
    private readonly int moveSpeedHash = Animator.StringToHash("moveSpeed");
    private const float AnimationDampTime = 0.1f;


    public PlayerAimingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.throwingEvent += OnThrow;
        stateMachine.InputReader.recallEvent += OnAimingRecall;
        stateMachine.animator.CrossFadeInFixedTime(AimingHash, stateMachine.aimBlendTime);
        
        if (stateMachine.crosshair != null)
        {
            stateMachine.crosshair.Show();
        }
    }


    public override void Tick(float deltaTime)
    {
        if (!stateMachine.InputReader.isAiming)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        float moveScale = stateMachine.aimMoveSpeedMultiplier;
        stateMachine.charController.Move(movement * stateMachine.moveSpeed * moveScale * deltaTime);

        if (stateMachine.InputReader.movementValue == Vector2.zero)
        {
            stateMachine.animator.SetFloat(moveSpeedHash, 0, AnimationDampTime, deltaTime);
            return;
        }

        FaceMovementDir(movement, deltaTime);
        stateMachine.animator.SetFloat(moveSpeedHash, 1, AnimationDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.throwingEvent -= OnThrow;
        stateMachine.InputReader.recallEvent -= OnAimingRecall;
        
        if (stateMachine.crosshair != null)
        {
            stateMachine.crosshair.Hide();
        }
    }
    private void OnThrow()
    {
        if(!stateMachine.AxeThrown)
             stateMachine.SwitchState(new PlayerThrowingState(stateMachine));
    }
    
    private void OnAimingRecall()
    {
        if (stateMachine.AxeThrown)
        {
            stateMachine.SwitchState(new PlayerRecallState(stateMachine));
        }
    }

    private Vector3 CalculateMovement()
    {
        Vector3 camForward = stateMachine.mainCam.transform.forward;
        Vector3 camRight = stateMachine.mainCam.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        var moveDir = camForward * stateMachine.InputReader.movementValue.y + camRight * stateMachine.InputReader.movementValue.x;
        return moveDir;
    }

    private void FaceMovementDir(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation,
            Quaternion.LookRotation(movement), deltaTime * stateMachine.rotationDamping);
    }

}
