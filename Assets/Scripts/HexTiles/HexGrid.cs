using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
	public const int HexCount = 19;

	public HexCell cellPrefab;
	public HexEdges edgePrefab;
	public HexVertices verticePrefab;
	public Text cellLabelPrefab;

	public HexCell[] cells;
	public List<HexVertices> hexVertices;
	public List<HexEdges> hexEdges;
	
	Canvas gridCanvas;
	


	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();


		cells = new HexCell[HexCount];
		CreateBoard();
	}
	void Start()
	{
		InitializeVerticesAndEdges();
	}
    void InitializeVerticesAndEdges()
    {
		for (int i = 0; i < HexCount; i++)
		{
			AddVerticesToHexagon(cells[i]);
			AddEdgesToVertices(cells[i]);
		}
	}
	void CreateBoard()
    {
		int i = 0;
		for (int j = -1; j < 2; j++)
		{
			CreateCell(j, 2, i++);
		}
		for (int j = -2; j < 2; j++)
		{
			CreateCell(j, 1, i++);
		}
		for (int j = -2; j < 3; j++)
		{
			CreateCell(j, 0, i++);
		}
		for (int j = -1; j < 3; j++)
		{
			CreateCell(j, -1, i++);
		}
		for (int j = -1; j < 2; j++)
		{
			CreateCell(j, -2, i++);
		}
	}
    void AddVerticesToHexagon(HexCell hexcell)
    {
		for (int i = 0; i < 6; i++)
        {
			Vector3 center = hexcell.transform.position;
			Vector3 expectedPosition = center + HexMetrics.corners[i];
			HexVertices vertice = ContainsVertice(expectedPosition);
			if (vertice == null)
            {
				HexVertices newVertice = Instantiate(verticePrefab);
				newVertice.transform.SetPositionAndRotation(expectedPosition, Quaternion.identity);
				hexVertices.Add(newVertice);
				hexcell.hexVertices.Add(newVertice);
            }
            else
            {
				hexcell.hexVertices.Add(vertice);
			}
		}
    }
	void AddEdgesToVertices(HexCell hexcell)
    {
		for(int i = 0; i < 6; i++)
        {
			Vector3 center = hexcell.transform.position;
			Vector3 expectedPosition = (center + HexMetrics.corners[i] + center + HexMetrics.corners[(i + 1) % 6]) / 2;
			HexEdges edge = ContainsEdge(expectedPosition);
			if(edge == null)
            {
				HexEdges newEdge = Instantiate(edgePrefab);
				newEdge.transform.SetPositionAndRotation(expectedPosition, Quaternion.identity);
				hexEdges.Add(newEdge);
				hexcell.hexVertices[i].connectedEdges.Add(newEdge);
				hexcell.hexVertices[(i + 1) % 6].connectedEdges.Add(newEdge);
				newEdge.connectedVertices[0] = hexcell.hexVertices[i];
				newEdge.connectedVertices[1] = hexcell.hexVertices[(i + 1) % 6];
			}
		}
    }
	HexVertices ContainsVertice(Vector3 position)
    {
		foreach(HexVertices vertice in hexVertices)
        {
			if (vertice.transform.position == position)
			{
				return vertice;
			}
        }
		return null;
    }
	HexEdges ContainsEdge(Vector3 position)
    {
		foreach(HexEdges edge in hexEdges)
        {
			if (edge.transform.position == position)
				return edge;
        }
		return null;
    }
	void CreateCell(int x, int z, int i)
	{
		// Position of the Cell
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);
		// Cell Part
		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.cellID = i;
        // Label Part
		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();

		
	}
}
