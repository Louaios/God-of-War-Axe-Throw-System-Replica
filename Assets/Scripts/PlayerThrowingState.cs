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
    private int framesSinceAnimStart;

    public PlayerThrowingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        hasThrown = false;
        hasFinished = false;
        animationStarted = false;
        delayTimer = 0f;
        framesSinceAnimStart = 0;
    }
    public override void Tick(float deltaTime)
    {
        // Wait for input delay
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

        // Wait a few frames for animation to be ready
        framesSinceAnimStart++;
        if (framesSinceAnimStart < 2)
        {
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
        if (animationStarted && !hasThrown && framesSinceAnimStart >= 2)
        {
            stateMachine.OnThrowStart();
        }
    }
}
