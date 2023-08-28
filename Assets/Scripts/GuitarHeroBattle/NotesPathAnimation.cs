using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesPathAnimation : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoints;
    private Vector2 gizmosPosition;

    public float pathSpeed;
    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = GetPositionOnPath(t);


            Gizmos.DrawSphere(gizmosPosition, 0.1f);
        }

        Gizmos.DrawLine(new Vector2(controlPoints[1].position.x, controlPoints[1].position.y),
            new Vector2(controlPoints[2].position.x, controlPoints[2].position.y));
    }

    public Vector2 GetPositionOnPath(float t)
    {
        return Mathf.Pow(1 - t, 3) * controlPoints[0].position +
                3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position +
                3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                Mathf.Pow(t, 3) * controlPoints[3].position;
    }
}
