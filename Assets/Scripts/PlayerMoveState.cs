using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private readonly int moveSpeedHash = Animator.StringToHash("moveSpeed");
    private readonly int moveHash = Animator.StringToHash("Locomotion");
    private const float animatorDampTime = 0.1f;

    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.animator.Play(moveHash);
    }
    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.isAiming)
        {
            stateMachine.SwitchState(new PlayerAimingState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement();
        stateMachine.charController.Move(movement * stateMachine.moveSpeed * deltaTime);

        if (stateMachine.InputReader.movementValue == Vector2.zero)
        {
            stateMachine.animator.SetFloat(moveSpeedHash, 0, animatorDampTime, deltaTime);
            return;
        }

        FaceMovementDir(movement, deltaTime);
        stateMachine.animator.SetFloat(moveSpeedHash, 1, animatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        
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
