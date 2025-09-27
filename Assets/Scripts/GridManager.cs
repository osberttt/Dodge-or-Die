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
}

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject playerPrefab;
    public Coord gridSize = new(5, 5);
    public float cellSpacing = 0.5f;
    public Cell[,] grid;
    public Player player;

    void Start()
    {
        GenerateGrid();
        SpawnPlayer();
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

        // Up
        if (c.y - 1 >= 0)
            neighbours.Add(grid[c.x, c.y - 1]);

        // Down
        if (c.y + 1 < gridSize.y)
            neighbours.Add(grid[c.x, c.y + 1]);

        // Left
        if (c.x - 1 >= 0)
            neighbours.Add(grid[c.x - 1, c.y]);

        // Right
        if (c.x + 1 < gridSize.x)
            neighbours.Add(grid[c.x + 1, c.y]);

        return neighbours.ToArray();
    }

    
}
