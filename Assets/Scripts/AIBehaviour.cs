using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public Transform[] WanderPoints;
    public Transform PreviousWanderPoint;
    public Transform Target;
    private void Update()
    {
        Wander();
    }
    private Vector3 Wander()
    {
        if (WanderPoints == null || WanderPoints.Length == 0) return Vector3.zero;
        if (Target == null)
        {
            int TargetNum = UnityEngine.Random.Range(0, WanderPoints.Length);
            Target = WanderPoints[TargetNum];
            PreviousWanderPoint = Target;
        }
        return FollowTargetVector();
    }
    private Vector3 FollowTargetVector()
    {
        if (Target == transform)
            Target = null;
        if (Target == null)
            return Vector3.zero;
        Vector3 newPos = Vector3.MoveTowards(transform.position, Target.transform.position, 1f);
        Vector3 offset = newPos - transform.position;
        return offset;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Target.position, 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(PreviousWanderPoint.position, 1f);

        Gizmos.color= Color.green;
        foreach(Transform transform in WanderPoints)
        {
            Gizmos.DrawSphere(transform.position, 1f);
        }

    }

}
