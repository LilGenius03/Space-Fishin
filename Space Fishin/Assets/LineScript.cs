using UnityEngine;

public class LineScript : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Transform[] linePoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = linePoints.Length;
        for (int i = 0; i < linePoints.Length; i++)
        {
            lineRenderer.SetPosition(i, linePoints[i].position);
        }
    }
}
