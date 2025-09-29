using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GridManager gridManager;
    public Coord currentCoord;
    public Cell[] neighbours;

    private RectTransform rect;
    private Image image;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void MoveTo(Cell cell)
    {
        currentCoord = cell.coord;
        if (cell.hasBooster)
        {
            UIManager.Instance.AddScore(5);
            Destroy(gridManager.booster);
            gridManager.booster = null;
        }
        UIManager.Instance.BlockInput();
        rect.DOAnchorPos(cell.rect.anchoredPosition, 0.5f).OnComplete(() => UIManager.Instance.UnblockInput());
        gridManager.UpdatePlayerNeighbours();
    }

    public void Die()
    {
        image.DOFade(0, 0.5f).OnComplete(() => UIManager.Instance.GameOver());
    }
}
