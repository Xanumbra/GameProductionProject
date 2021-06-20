using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public LineRenderer lineRenderer;


    public Vector3[] HexEdges;
    public List<HexVertices> hexVertices = new List<HexVertices>(6);

    public int cellID;
    public Enums.Resources cellResourceType;
    public int cellDiceNumber;

    public bool hasSpacePirates;
    public bool hadSpacePiratesBefore;

    public void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        HexEdges = new Vector3[7];
        SetupLineRenderer();
    }
    void SetupLineRenderer()
    {
        lineRenderer.startWidth = 0.10f;
        lineRenderer.endWidth = 0.10f;
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }
    void Start()
    {
        DrawHexagon(this);
    }
    void FillHexagonVertices(HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        for (int i = 0; i < HexMetrics.corners.Length; i++)
        {
            HexEdges[i] = center + HexMetrics.corners[i];
        }
    }
    void DrawHexagon(HexCell cell)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = HexMetrics.corners.Length;
        FillHexagonVertices(cell);
        lineRenderer.SetPositions(HexEdges);
    }
}
