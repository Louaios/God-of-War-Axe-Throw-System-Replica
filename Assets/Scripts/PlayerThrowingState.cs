using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowingState : PlayerBaseState
{
    private readonly int throwHash = Animator.StringToHash("Throwing");
    private bool hasThrown;
    private bool hasFinished;
    private bool animationStarted;
    private float delayTimer;

    public PlayerThrowingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        hasThrown = false;
        hasFinished = false;
        animationStarted = false;
        delayTimer = 0f;
    }
    public override void Tick(float deltaTime)
    {
        if (!animationStarted)
        {
            delayTimer += deltaTime;
            if (delayTimer < stateMachine.throwInputDelay)
            {
                return;
            }

            animationStarted = true;
            stateMachine.animator.CrossFadeInFixedTime(throwHash, 0.05f);
            return;
        }

        AnimatorStateInfo info = stateMachine.animator.GetCurrentAnimatorStateInfo(0);

        float throwStart = Mathf.Clamp01(stateMachine.throwStartNormalizedTime);
        float throwEnd = Mathf.Clamp01(stateMachine.throwEndNormalizedTime);

        if (!hasThrown && info.normalizedTime >= throwStart)
        {
            stateMachine.OnThrowStart();
            hasThrown = true;
        }

        if (!hasFinished && info.normalizedTime >= throwEnd)
        {
            hasFinished = true;
            stateMachine.OnThrowFinish();
        }
    }

    public override void Exit()
    {
        if (animationStarted && !hasThrown)
        {
            stateMachine.OnThrowStart();
        }
    }

}
