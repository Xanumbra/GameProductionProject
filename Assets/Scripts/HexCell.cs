using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public LineRenderer lineRenderer;


    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        DrawHexagon(this);
    }

    Vector3[] HexagonVertices(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        Vector3[] hexagonVectices = new Vector3[HexMetrics.corners.Length];
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            hexagonVectices[i] = center + HexMetrics.corners[i];
        }
        return hexagonVectices;
    }
    void DrawHexagon(HexCell cell)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = HexMetrics.corners.Length;
        lineRenderer.SetPositions(HexagonVertices(cell));

    }
}
