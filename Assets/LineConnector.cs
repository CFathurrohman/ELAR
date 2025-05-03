using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Vector3 offsetA = Vector3.zero;
    public Vector3 offsetB = Vector3.zero;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            Debug.LogError("LineRenderer not found!");
    }

    void Update()
    {
        if (pointA != null && pointB != null)
        {
            lineRenderer.SetPosition(0, pointA.position + pointA.TransformDirection(offsetA));
            lineRenderer.SetPosition(1, pointB.position + pointB.TransformDirection(offsetB));
        }
        else
        {
            Debug.LogWarning("pointA or pointB belum di-assign!");
        }
    }
}
