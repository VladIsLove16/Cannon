using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Cinemachine.CinemachineTargetGroup;
using static Mover;
using static UnityEngine.GraphicsBuffer;

public class Mover : MonoBehaviour
{
    public enum MoveVectorStrategyRealizationType
    {
        MonoBehaviours,
        Methods,
        Interfaces
    }
    public enum MoveVectorStrategyMethod
    {
        FollowTarget,
        Input,
        AI
    }
    public enum MoveMethod
    {
        Physic,
        Transform
    }

    public MoveVectorStrategyRealizationType moveStrategyRealizationType;
    public MoveVectorStrategyMethod moveStrategy;
    public MoveMethod moveMethod;
    [SerializeField]
    public Vector3 MoveDirection {  get; set; }
    private float CurrentMovespeed;
    [SerializeField]
    public float Movespeed;
    [SerializeField]
    public float Force;
    public float Turnspeed = 0.1f;
    private float TurnTime=0.1f;
    [SerializeField]
    public Transform Target;
    public Transform[] WanderPoints;
    public Transform PreviousWanderPoint;
    Rigidbody rb;
    [SerializeField]
    InputActionReference Movement;
    private Vector3 movementInput { get; set; }
    

    private MovementStrategy movementStrategy;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        movementStrategy=GetComponent<MovementStrategy>();
    }
    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        if(Movement!=null) movementInput = Movement.action.ReadValue<Vector3>();
        MoveDirection = MoveVector().normalized;
        SpeedControl();
        switch (moveMethod)
        {
            case MoveMethod.Physic:
                {
                    PhysicsMove();
                    break;
                }
            case MoveMethod.Transform:
                {
                    TransformMove();
                    break;
                }
            default: break;
        }
    }

    private void Rotate()
    {
        if(MoveDirection.magnitude>0.1f)
        {
            float targetAngle = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg;
            float rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref Turnspeed, TurnTime);
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }
    }

    private Vector3 MoveVector()
    {
        switch(moveStrategyRealizationType)
        {
            case MoveVectorStrategyRealizationType.MonoBehaviours:
                {
                    return movementStrategy.Direction;
                }
            case MoveVectorStrategyRealizationType.Methods:
                switch (moveStrategy)
                {
                    case MoveVectorStrategyMethod.AI:
                        {
                            return Wander();
                            break;
                        }
                    case MoveVectorStrategyMethod.FollowTarget:
                        {
                            return FollowTargetVector();
                            break;
                        }
                    case MoveVectorStrategyMethod.Input:
                        {
                            if (movementInput.magnitude>0.1f)
                            {
                                float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                                Vector3 movementDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                                return movementDirection;
                            }
                            else return Vector3.zero;
                            break;
                        }
                    default:
                        return Vector3.zero;
                }
            case MoveVectorStrategyRealizationType.Interfaces:
                throw new NotImplementedException();    
            default :
                return Vector3.zero;
        }
    }
    private Vector3 Wander()
    {
        if(WanderPoints==null || WanderPoints.Length==0) return Vector3.zero;
        if (Target == null)
        {
            List<Transform> AccesiblePoints = WanderPoints.ToList();
            AccesiblePoints.Remove(PreviousWanderPoint);
            int TargetNum = UnityEngine.Random.Range(0, AccesiblePoints.Count);
            Target = AccesiblePoints[TargetNum];
        }
        return FollowTargetVector();
    }
    private Vector3 FollowTargetVector()
    {

        if (Target == null)
            return Vector3.zero;
        //                                                 без учета y координаты
        if (Vector3.Distance(transform.position, Target.position)-MathF.Abs(transform.position.y- Target.position.y) <= 1f)
        {
            PreviousWanderPoint = Target;   
            Target = null;
            return Vector3.zero;
        }
        Vector3 newPos = Vector3.MoveTowards(transform.position, Target.transform.position, 1f);
        Vector3 offset = newPos - transform.position;
        return offset;
    }
    private void PhysicsMove()
    {
        rb.AddForce(new Vector3(MoveDirection.x, 0, MoveDirection.z) * Movespeed * Force, ForceMode.Force); ;   
        CurrentMovespeed = rb.velocity.magnitude;
    }
    private void TransformMove()
    {
        Vector3 newPos = transform.localPosition + new Vector3(Movespeed * Time.deltaTime * MoveDirection.x, 0,  Movespeed * Time.deltaTime * MoveDirection.z);
        transform.localPosition = newPos;
    }
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVelocity.magnitude>Movespeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * Movespeed;
            rb.velocity = new Vector3(limitedVelocity.x,rb.velocity.y, limitedVelocity.z);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if(Target != null)
        Gizmos.DrawSphere(Target.position, 1f);

        Gizmos.color = Color.red;
        if (PreviousWanderPoint != null)
            Gizmos.DrawSphere(PreviousWanderPoint.position, 1f);

        Gizmos.color = Color.green;
        foreach (Transform transform in WanderPoints)
        {
            if(transform!= PreviousWanderPoint || transform != Target)
                Gizmos.DrawSphere(transform.position, 1f);
        }

    }
    //private void FollowCursor_Action_Started(UnityEngine.InputSystem.InputAction.CallbackContext context)
    //{

    //    target.position = pointerInput;
    //    currentHoldTime += Time.deltaTime;
    //}

    //private void FollowCursor_Action_Canceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    //{
    //    if (currentHoldTime > maxHoldTime)
    //        target = null;
    //    currentHoldTime = 0;
    //}

    //private void FollowCursor_Action_Performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    //{
    //    target.position = pointerInput;
    //}
}
