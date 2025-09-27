using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GridManager gridManager;
    public Coord currentCoord;
    public Cell[] neighbours;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void MoveTo(Cell cell)
    {
        currentCoord = cell.coord;
        rect.anchoredPosition = cell.rect.anchoredPosition;
        gridManager.UpdatePlayerNeighbours();
    }
}
