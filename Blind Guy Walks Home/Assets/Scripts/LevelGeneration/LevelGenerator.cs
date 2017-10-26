using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 10/25/2017
/// Purpose: Holds data to manage level generation and generates the level.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    #region LevelGenerator Members
    public int m_width;                 // The width of the map
    public int m_height;                // The height of the map

    public int m_obstacleDistance;      // The minimum distance between each obstacle
    public int m_obstacleCount;         // How many obstacles do we want to place
    public int m_maxAttempts;           // Caps attempts at placing obstacles

    public int m_monsterDistance;       // The starting minimum distance from the player

    [HideInInspector] public Vector3 m_playerStart;     // Store where the player starts
    [HideInInspector] public Vector3 m_monsterStart;    // Store where the monster starts

    private List<List<Cell>> m_grid;    // The digital data for the entire level
    public List<List<Cell>> Grid
    {
        get { return m_grid; }
    }
    #endregion

    #region LevelGenerator Methods
    private void Start()
    {
        m_grid = new List<List<Cell>>();
    }

    public void GenerateLevel()
    {
        // Prepare for level generation
        InstantiateGrid();

        // Generate the monster starting point
        int monsterX = Random.Range(0, m_width);
        int monsterY = Random.Range(1, 4);
        m_monsterStart = new Vector3(monsterX, monsterY);

        // Generate the obstacles
        int attempts = 0;
        int obstacles = 0;
        while (attempts < m_maxAttempts)
        {
            // Generate a random obstacle position
            int obstacleX = Random.Range(0, m_width);
            int obstacleY = Random.Range(0, m_height);

            // See if there is an obstacle here or nearby
            if (NearbyObstacle(obstacleY, obstacleX, m_obstacleDistance))
            {
                attempts++;
                continue;
            }
            else
            {
                // Generate a random obstacle type
                int type = Random.Range(0, 2);

                // Generate an obstacle at that location and save it
                switch (type)
                {
                    case 0: m_grid[obstacleY][obstacleX].Type = Cell.LevelType.Tree; break;
                    case 1: m_grid[obstacleY][obstacleX].Type = Cell.LevelType.Rock; break;
                }
                attempts++;
                obstacles++;
            }

            // Create no more obstacles if we created enough
            if (obstacles > m_obstacleCount)
                break;
        }

        // Generate the player position
        int playerX;
        int playerY;
        while (true)
        {
            playerX = Random.Range(0, m_width);
            playerY = Random.Range(2 + m_monsterDistance, 6 + m_monsterDistance);

            // Break once we find a valid position that doesn't have an obstacle
            if (PositionExists(playerY, playerX) &&
                !m_grid[playerY][playerX].Obstacle)
                break;
        }
        m_playerStart = new Vector3(playerX, playerY);
    }

    private void InstantiateGrid()
    {
        for (int i = 0; i < m_height; i++)
        {
            // Add a row
            m_grid.Add(new List<Cell>());

            // Add a cell for each element in the row
            for (int j = 0; j < m_width; j++)
                m_grid[i].Add(new Cell(i, j));
        }
    }

    private bool NearbyObstacle(int y, int x, int distance)
    {
        for (int i = -distance; i < distance; i++)
            for (int j = -distance; j < distance; j++)
            {
                if (PositionExists(i, j))
                    if (m_grid[i][j].Type == Cell.LevelType.Tree ||
                        m_grid[i][j].Type == Cell.LevelType.Rock)
                        return true;
            }

        return false;
    }

    private bool PositionExists(int y, int x)
    {
        return y >= 0 && y < m_height &&
               x >= 0 && x < m_width;
    }
    #endregion
}
