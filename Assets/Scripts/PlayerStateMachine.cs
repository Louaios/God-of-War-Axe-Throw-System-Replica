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

    public bool AxeThrown;

    private Rigidbody rbAxe;
    private Vector3 axeOldPos;
    private bool isReturning = false;
    private float time = 0.0f;

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

        if(isReturning)
        {
            if (time < 1.0f)
            {
                Axe.position = getBezierCurvePoint(time, axeOldPos, curve_Point.position, target.position);
                Axe.rotation = Quaternion.Slerp(Axe.transform.rotation,target.transform.rotation, 50 * Time.deltaTime);
                time += Time.deltaTime;
            }else
            {
                ResetAxe();
            }
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

    public void RecallAxe()
    {
        time = 0;
        axeOldPos = Axe.transform.position;
        isReturning = true;
        rbAxe.velocity = Vector3.zero;
        rbAxe.isKinematic = true;
        AxeThrown = false;
    }

    public void ResetAxe()
    {
        isReturning = false;
        Axe.position = target.position;
        Axe.rotation = target.rotation;
        Axe.transform.parent = target;
    }

    private Vector3 getBezierCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }
}
