using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecallState : PlayerBaseState
{
    private readonly int recallHash = Animator.StringToHash("Recall");
    private Vector3 axeStartPos;
    private float time;
    private bool arrived;
    private float delayTimer;

    public PlayerRecallState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        time = 0f;
        arrived = false;
        delayTimer = 0f;
        axeStartPos = stateMachine.Axe.position;

        stateMachine.AxeRigidbody.linearVelocity = Vector3.zero;
        stateMachine.AxeRigidbody.isKinematic = true;
        stateMachine.Axe.SetParent(null);

        stateMachine.animator.CrossFadeInFixedTime(recallHash, stateMachine.recallBlendTime);
    }

    public override void Tick(float deltaTime)
    {
        if (!arrived)
        {
            float duration = Mathf.Max(0.01f, stateMachine.recallDuration);
            float t = Mathf.Clamp01(time / duration);

            stateMachine.Axe.position = GetBezierPoint(t, axeStartPos, stateMachine.curve_Point.position, stateMachine.target.position);
            
            // Apply spinning rotation like during throw
            Vector3 localZAxis = stateMachine.Axe.TransformDirection(Vector3.forward);
            float spinAmount = stateMachine.throwSpinSpeed * deltaTime;
            stateMachine.Axe.Rotate(localZAxis, spinAmount, Space.World);

            if (t >= 1f)
            {
                arrived = true;
                stateMachine.AxeThrown = false;
                stateMachine.ResetAxe();
            }

            time += deltaTime;
            return;
        }

        delayTimer += deltaTime;
        if (delayTimer >= stateMachine.recallEndDelay)
        {
            stateMachine.SwitchState(new PlayerAimingState(stateMachine));
        }
    }

    public override void Exit()
    {
        AxeCollisionHandler axeCollider = stateMachine.Axe.GetComponent<AxeCollisionHandler>();
        if (axeCollider != null)
        {
            axeCollider.ResetHit();
        }
    }

    private Vector3 GetBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2f * u * t * p1) + (tt * p2);
    }
}
