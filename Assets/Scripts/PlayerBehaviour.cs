using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private float pace = 10f, maxSwerveSpeed = 15f, swerveLimit = 7.5f;

    private IInputRaiser inputRaiser;

    private Animator animator;
    private int isMovingParamId; //id of the isMoving parameter

    private Vector3 pos;
    private float swerveMag;

    State state = State.Running;
    private void Awake()
    {
        inputRaiser = gameObject.GetComponent<IInputRaiser>();
        animator = gameObject.GetComponent<Animator>();
        isMovingParamId = Animator.StringToHash("isMoving");
    }
    private void Start()
    {
        inputRaiser.Swerve += Swerve;
    }
    private void Update()
    {
        if(state == State.Running)
        {
            MoveForward();
        }
    }
    private void MoveForward()
    {
        transform.position += pace * Time.deltaTime * Vector3.forward;
    }
    private void Swerve(float swerveInput)
    {
        swerveMag = swerveInput * maxSwerveSpeed * Time.deltaTime;

        pos = transform.position;

        transform.position = new Vector3(Clamp(pos.x + swerveInput * maxSwerveSpeed * Time.deltaTime, -swerveLimit, swerveLimit), pos.y, pos.z);
    }
    
    private enum State { Running, Idle }
}
