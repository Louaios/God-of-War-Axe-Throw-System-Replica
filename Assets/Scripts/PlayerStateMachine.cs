using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field : SerializeField] public InputReader InputReader {  get; private set; }
    [field: SerializeField] public CharacterController charController { get; private set; }

    [field: SerializeField] public float moveSpeed { get; private set; }

    [field: SerializeField] public float rotationDamping { get; private set; }

    [field: SerializeField] public Animator animator { get; private set; }

    [field: SerializeField] public Transform Axe { get; private set; }

    [field: Header("Throwing")]

    [field: SerializeField] public Transform curve_Point { get; private set; }

    [field: SerializeField] public Transform target { get; private set; }

    [field: SerializeField] public float throwForce { get; private set; }

    [field: SerializeField] public float rotationSpeed { get; private set; }

    [field: SerializeField] public float throwSpinSpeed { get; private set; } = 1080f;

    [field: SerializeField] public float throwInputDelay { get; private set; } = 0.1f;

    [field: SerializeField] public float throwStartNormalizedTime { get; private set; } = 0.35f;

    [field: SerializeField] public float throwEndNormalizedTime { get; private set; } = 0.9f;

    [field: SerializeField] public float recallDuration { get; private set; } = 1.5f;

    [field: SerializeField] public float recallEndDelay { get; private set; } = 0.2f;

    [field: SerializeField] public float recallBlendTime { get; private set; } = 0.15f;

    [field: SerializeField] public float aimBlendTime { get; private set; } = 0.12f;

    [field: SerializeField] public float moveBlendTime { get; private set; } = 0.12f;

    [field: SerializeField] public float aimMoveSpeedMultiplier { get; private set; } = 0.6f;

    [field: SerializeField] public float maxAxeAngularSpeed { get; private set; } = 100f;

    [field: SerializeField] public CrosshairUI crosshair { get; private set; }

    public bool AxeThrown;

    private Rigidbody rbAxe;
    private Vector3 axeLocalPosition;
    private Quaternion axeLocalRotation;

    public Rigidbody AxeRigidbody => rbAxe;

    public Transform mainCam { get; private set; }

    void Start()
    {
        mainCam = Camera.main.transform;
        rbAxe = Axe.GetComponent<Rigidbody>();
        rbAxe.isKinematic = true;
        rbAxe.maxAngularVelocity = maxAxeAngularSpeed;
        axeLocalPosition = Axe.localPosition;
        axeLocalRotation = Axe.localRotation;
        SwitchState(new PlayerMoveState(this));
    }


    void Update()
    {
        currentState.Tick(Time.deltaTime);

    }

    private void FixedUpdate()
    {
        if (AxeThrown && !rbAxe.isKinematic)
        {
            Vector3 localZAxis = rbAxe.transform.TransformDirection(Vector3.forward);
            rbAxe.angularVelocity = localZAxis * throwSpinSpeed * Mathf.Deg2Rad;
        }
    }

    public void OnThrowStart()
    {
        AxeThrown = true;
        rbAxe.isKinematic = false;
        Axe.SetParent(null);

        Axe.rotation = Quaternion.LookRotation(mainCam.transform.forward, Vector3.up);
        Axe.Rotate(Vector3.up, 90f, Space.Self);

        Vector3 forceToAdd = mainCam.transform.forward * throwForce;
        rbAxe.AddForce(forceToAdd, ForceMode.Impulse);
    }

    public void OnThrowFinsih()
    {
        SwitchState(new PlayerAimingState(this));
    }

    public void OnThrowFinish()
    {
        OnThrowFinsih();
    }

    public void ResetAxe()
    {
        Axe.SetParent(target, false);
        Axe.localPosition = axeLocalPosition;
        Axe.localRotation = axeLocalRotation;
    }

}