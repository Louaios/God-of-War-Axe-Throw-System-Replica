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

    [field: SerializeField] public float throwForce { get; private set; }

    [field: SerializeField] public float rotationSpeed { get; private set; }

    public bool AxeThrown;

    private Rigidbody rbAxe;

    public Transform mainCam { get; private set; }

    void Start()
    {
        mainCam = Camera.main.transform;
        rbAxe = Axe.GetComponent<Rigidbody>();
        rbAxe.isKinematic = true;
        SwitchState(new PlayerMoveState(this));
    }


    void Update()
    {
        currentState.Tick(Time.deltaTime);

    }

    private void FixedUpdate()
    {
        if (AxeThrown)
        {
            Axe.transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }        
    }

    public void OnThrowStart()
    {
        AxeThrown = true;
        rbAxe.isKinematic = false;
        Axe.SetParent(null);

        Vector3 forceToAdd = mainCam.transform.forward * throwForce;

        rbAxe.AddForce(forceToAdd, ForceMode.Impulse);
        Axe.transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
    }

    public void OnThrowFinsih()
    {
        SwitchState(new PlayerAimingState(this));
    }
}
