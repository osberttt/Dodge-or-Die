using System.Collections.Generic;
using UnityEngine;


public struct Coord
{
    public int x;
    public int y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public static bool operator ==(Coord a, Coord b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Coord a, Coord b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is Coord other)
        {
            return this == other;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // Simple hash function combining x and y
        return (x * 397) ^ y;
    }
}

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject boosterPrefab;
    public Coord gridSize = new(4, 4);
    public float cellSpacing = 120f;
    public Cell[,] grid;
    public Player player;
    public List<Enemy> enemies;
    public int enemySpawnCD = 4;
    public int boosterSpawnCD = 2;
    public GameObject booster;

    void Start()
    {
        GenerateGrid();
        SpawnPlayer();
    }

    private void OnEnable()
    {
        GameManager.Instance.onCompleteTurn.AddListener(DecideSpawnEnemy);
        GameManager.Instance.onCompleteTurn.AddListener(DecideSpawnBooster);
    }

    private void OnDisable()
    {
        GameManager.Instance.onCompleteTurn.RemoveListener(DecideSpawnEnemy);
        GameManager.Instance.onCompleteTurn.RemoveListener(DecideSpawnBooster);
    }
    
    private void SpawnPlayer()
    {
        var playerObj = Instantiate(playerPrefab, transform);
        
        var playerCoord = new Coord(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        
        var rt = playerObj.GetComponent<RectTransform>();
        rt.anchoredPosition = grid[playerCoord.x, playerCoord.y].rect.anchoredPosition;
        
        var playerComponent = playerObj.GetComponent<Player>();
        playerComponent.name = "Player";
        playerComponent.currentCoord = playerCoord;
        playerComponent.gridManager = this;
        player = playerComponent;

        foreach (var cell in grid)
        {
           cell.player = player; 
        }
        
        UpdatePlayerNeighbours();
    }

    private void DecideSpawnEnemy(int score)
    {
        var chance = Random.Range(0, enemySpawnCD);
        if (chance == 0)
        {
            SpawnEnemy();
        }
    }
    
    private void SpawnEnemy()
    {
        var enemyObj = Instantiate(enemyPrefab, transform);
        
        var index = Random.Range(0, 3);
        var xCoord = 0;
        var yCoord = 0;
        switch (index)
        {
            case 0:
                xCoord = Random.Range(0, 2) == 0 ? gridSize.x - 1 : 0;
                yCoord = Random.Range(0, 2) == 0 ? gridSize.y - 1 : 0;
                break;
            case 1:
                xCoord = Random.Range(0, 2) == 0 ? gridSize.x - 1 : 0;
                yCoord = Random.Range(0, gridSize.y - 1);
                break;
            case 2:
                xCoord = Random.Range(0, gridSize.x - 1);
                yCoord = Random.Range(0, 2) == 0 ? gridSize.y - 1 : 0;
                break;
        }
        var enemyCoord = new Coord(xCoord, yCoord);
        var rt = enemyObj.GetComponent<RectTransform>();
        rt.anchoredPosition = grid[enemyCoord.x, enemyCoord.y].rect.anchoredPosition;
        var enemyComponent = enemyObj.GetComponent<Enemy>();
        enemyComponent.name = "Enemy";
        enemyComponent.currentCoord = enemyCoord;
        enemyComponent.gridManager = this;
        enemyComponent.SetDirection();
        enemyComponent.SetActive(false);
        enemyComponent.player = player;
        enemies.Add(enemyComponent);
    }

    private void DecideSpawnBooster(int score)
    {
        if (booster) return;
        var chance = Random.Range(0, boosterSpawnCD);
        if (chance == 0)
        {
            SpawnBooster();
        }
    }

    private void SpawnBooster()
    {
        var boosterObj = Instantiate(boosterPrefab, transform);
        booster = boosterObj;
        var boosterCoord = player.currentCoord;
        while (boosterCoord == player.currentCoord)
        {
            boosterCoord = new Coord(Random.Range(0, gridSize.x), Random.Range(0, gridSize.y));
        }

        var boosterCell = grid[boosterCoord.x, boosterCoord.y];
        boosterCell.hasBooster = true;
        var rt = boosterObj.GetComponent<RectTransform>();
        rt.anchoredPosition = boosterCell.rect.anchoredPosition;
    }
    private void GenerateGrid()
    {
        grid = new Cell[gridSize.x, gridSize.y];

        for (var y = 0; y < gridSize.x; y++)
        {
            for (var x = 0; x < gridSize.y; x++)
            {
                // instantiate cell prefab
                var newCell = Instantiate(cellPrefab, transform);
                
                // rename cell object
                newCell.name = "Cell [" + x + "," + y + "]";
                
                // Set position in UI space
                var rt = newCell.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(x * cellSpacing, -y * cellSpacing);

                // get a random number between 1 and 6
                var number = Random.Range(1, 7);
                
                // Pass data to cell script
                var cell = newCell.GetComponent<Cell>();
                if (cell != null)
                {
                    cell.Init(x, y, number);
                }
                
                // add cell to grid
                grid[x, y] = cell;
            }
        }
    }

    public void UpdatePlayerNeighbours()
    {
        player.neighbours = GetPlayerNeighbours();
        foreach (var cell in grid)
        {
            cell.image.color = new Color(cell.image.color.r, cell.image.color.g, cell.image.color.b, 0.5f);
        }
        foreach (var cell in player.neighbours)
        {
            cell.image.color = new Color(cell.image.color.r, cell.image.color.g, cell.image.color.b, 1);
        }
    }

    private Cell[] GetPlayerNeighbours()
    {
        var neighbours = new System.Collections.Generic.List<Cell>();
        var c = player.currentCoord;

        // Loop through offsets (-1, 0, 1) for both x and y
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                // Skip the player's own position (0,0 offset)
                if (dx == 0 && dy == 0)
                    continue;

                int nx = c.x + dx;
                int ny = c.y + dy;

                // Check grid bounds
                if (nx >= 0 && nx < gridSize.x && ny >= 0 && ny < gridSize.y)
                {
                    neighbours.Add(grid[nx, ny]);
                }
            }
        }

        return neighbours.ToArray();
    }


    
}
