using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public GridManager gridManager;
    public Coord currentCoord;
    public Vector2 direction;
    public Player player;

    private RectTransform rect;

    private bool isActive = true;
    public Image[] images;
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.onCompleteTurn.AddListener(Move);
    }

    private void OnDisable()
    {
        GameManager.Instance.onCompleteTurn.RemoveListener(Move);
    }

    public void SetDirection()
    {
        /*var unavailableCoords = new List<int>();
        if (currentCoord.y == 0) unavailableCoords.Add(0);
        if (currentCoord.y == gridManager.gridSize.y - 1) unavailableCoords.Add(1);
        if (currentCoord.x == 0) unavailableCoords.Add(2);
        if (currentCoord.x == gridManager.gridSize.x - 1) unavailableCoords.Add(3);

        var index = -1;
        while (index == -1)
        {
            index = Random.Range(0, 4);
            if (unavailableCoords.Contains(index)) index = -1;
        }*/
        
        var index = Random.Range(0, 4);
        switch (index)
        {
            case 0:
                direction = Vector2.left;
                transform.rotation = Quaternion.Euler(0,0,0);// (-1, 0)
                break;
            case 1:
                direction = Vector2.right;
                transform.rotation = Quaternion.Euler(0,0,180);// (1, 0)
                break;
            case 2:
                direction = Vector2.up;
                transform.rotation = Quaternion.Euler(0,0,270);// (0, 1)
                break;
            case 3:
                direction = Vector2.down;
                transform.rotation = Quaternion.Euler(0,0,90);// (0, -1)
                break;
        }
    }

    public void SetActive(bool active)
    {
        if (active && !isActive)
        {
            foreach (var image in images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            }
        }
        if (isActive && !active)
        {
            foreach (var image in images)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            }
        }
        isActive = active;
    }
    private void Move(int score)
    {
        if (!isActive)
        {
            SetActive(true);
            return;
        }
        var newCoord = new Coord(currentCoord.x + (int)direction.x, currentCoord.y - (int)direction.y);
        if (newCoord.x < 0 || newCoord.y < 0 || newCoord.x >= gridManager.gridSize.x ||
            newCoord.y >= gridManager.gridSize.y)
        {
            gridManager.enemies.Remove(this);
            Destroy(this.gameObject);
            return;
        }
        currentCoord = newCoord;
        var newCell = gridManager.grid[newCoord.x, newCoord.y];
        UIManager.Instance.BlockInput();
        rect.DOAnchorPos(newCell.rect.anchoredPosition, 0.5f).OnComplete(() => UIManager.Instance.UnblockInput());

        if (player.currentCoord == currentCoord)
        {
            player.Die();
        }
    }
}
