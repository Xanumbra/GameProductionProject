﻿using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{

	public int width = 6;
	public int height = 6;

	public const int HexCount = 19;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	HexCell[] cells;
	Canvas gridCanvas;
	HexMesh hexMesh;


	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();

		cells = new HexCell[HexCount];
		CreateBoard();
	}
	void Start()
	{
		hexMesh.Triangulate(cells);
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
	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}
