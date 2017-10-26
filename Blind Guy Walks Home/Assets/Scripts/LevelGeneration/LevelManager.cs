using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Dante Nardo
/// Last Modified: 10/25/2017
/// Purpose: Holds data to manage level generation and generates the level.
/// </summary>
[RequireComponent(typeof(LevelGenerator))]
public class LevelManager : MonoBehaviour
{
    #region LevelManager Members
    private LevelGenerator m_generator;

    public float m_scaleFactor;         // Factor to scale grid -> world positions

    public GameObject m_floor;          // Plane in level
    public GameObject m_treeObject;     // Tree prefab
    public GameObject m_rockObject;     // Rock prefab
    public GameObject m_playerObject;   // Player prefab
    public GameObject m_monsterObject;  // Monster prefab

    private List<GameObject> m_rocks;   // A list that stores all instantiated rocks
    private List<GameObject> m_trees;   // A list that stores all instantiated treess
    private GameObject m_player;        // Stores the player object
    private GameObject m_monster;       // Stores the monster object
    #endregion

    #region LevelManager Methods
    void Start()
    {
        m_rocks = new List<GameObject>();
        m_trees = new List<GameObject>();
        m_generator = GetComponent<LevelGenerator>();
        m_generator.GenerateLevel();
        SpawnLevel();
        SpawnPlayer();
        SpawnMonster();
    }

    private void SpawnLevel()
    {
        // Spawn the floor
        m_floor.transform.localScale = new Vector3(m_generator.m_width, 0, m_generator.m_height);
        m_floor.transform.Translate(new Vector3(m_generator.m_width * 5, 0, m_generator.m_height * 5));

        // Spawn the obstacles in the level
        Cell c;
        for (int i = 0; i < m_generator.m_height; i++)
            for (int j = 0; j < m_generator.m_width; j++)
            {
                c = m_generator.Grid[i][j];
                switch (c.Type)
                {
                    case Cell.LevelType.Rock:
                        var rock = Instantiate(m_rockObject, new Vector3(c.X * m_scaleFactor, 0, c.Y * m_scaleFactor), Quaternion.identity);
                        rock.transform.Translate(new Vector3(m_generator.m_width / 2, 0, m_generator.m_height / 2));
                        m_rocks.Add(rock);
                        break;
                    case Cell.LevelType.Tree:
                        var tree = Instantiate(m_treeObject, new Vector3(c.X * m_scaleFactor, 0, c.Y * m_scaleFactor), Quaternion.identity);
                        tree.transform.Translate(new Vector3(m_generator.m_width / 2, 0, m_generator.m_height / 2));
                        m_trees.Add(tree);
                        break;
                }
            }
    }

    private void SpawnPlayer()
    {
        Vector3 playerPos = new Vector3(
            m_generator.m_playerStart.x, 1, 
            m_generator.m_playerStart.y);
        m_player = Instantiate(m_playerObject, playerPos, Quaternion.identity);
    }

    private void SpawnMonster()
    {
        Vector3 monsterPos = new Vector3(
            m_generator.m_monsterStart.x, 1, 
            m_generator.m_monsterStart.y);
        m_monster = Instantiate(m_monsterObject, monsterPos, Quaternion.identity);
    }
    #endregion
}
