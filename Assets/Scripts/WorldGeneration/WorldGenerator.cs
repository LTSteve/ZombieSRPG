using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldGenerator : MonoBehaviour
{
    public TileGenerator TilePrefab;
    public EnemyBehaviour EnemyPrefab;
    public PlayerBehaviour PlayerPrefab;

    public NavMeshSurface Navigation;

    public int tilesWide;
    public int tilesHigh;

    void Start()
    {
        var tiles = new TileGenerator[tilesWide, tilesHigh];
        var firstTile = true;

        for(var i = 0; i < tilesWide; i++)
            for(var j = 0; j < tilesHigh; j++)
            {
                tiles[i, j] = Instantiate(TilePrefab, new Vector3(i, 0, j) * 10, Quaternion.identity, transform);
                tiles[i, j].GenerateWalls();
            }

        Navigation.BuildNavMesh();

        for (var i = 0; i < tilesWide; i++)
            for (var j = 0; j < tilesHigh; j++)
            {
                tiles[i, j].GenerateEnemies(EnemyPrefab, PlayerPrefab, firstTile);

                if (firstTile)
                {
                    firstTile = !firstTile;
                }
            }
    }
}
