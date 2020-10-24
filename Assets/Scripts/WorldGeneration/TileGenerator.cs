using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public Transform WallObject;

    public float TileWidth;
    public float TileHeight;
    public float WallSpaceRate;
    public float EnemySpaceRate;

    private List<Vector2Int> wallLocations;

    public void GenerateEnemies(EnemyBehaviour EnemyPrefab, PlayerBehaviour PlayerPrefab, bool playerToo)
    {
        var spaces = TileWidth * TileHeight;
        var wallSpaces = (int)(spaces * WallSpaceRate);
        var wallsTransform = transform.Find("Walls");
        var transformOffset = transform.position - new Vector3(TileWidth / 2f - 0.5f, 0, TileHeight / 2f - 0.5f);
        var enemySpaces = (int)(spaces * EnemySpaceRate);
        for (var i = wallSpaces; i < (wallSpaces + enemySpaces) && i < wallLocations.Count; i++)
        {
            Instantiate(EnemyPrefab, new Vector3(wallLocations[i].x, 0, wallLocations[i].y) + transformOffset, Quaternion.identity);
        }

        if (playerToo)
        {
            var spot = ((wallSpaces + enemySpaces) > wallLocations.Count) ? wallSpaces : (wallSpaces + enemySpaces);

            Instantiate(PlayerPrefab, new Vector3(wallLocations[spot].x, 0, wallLocations[spot].y) + transformOffset, Quaternion.identity);
        }
    }

    public void GenerateWalls()
    {
        wallLocations = new List<Vector2Int>();

        for (var i = 0; i < TileWidth; i++)
        {
            for (var j = 0; j < TileHeight; j++)
            {
                wallLocations.Add(new Vector2Int(i, j));
            }
        }

        //randomize
        for (var i = 0; i < wallLocations.Count; i++)
        {
            var randomLocation = Random.Range(0, wallLocations.Count);

            var temp = wallLocations[i];
            wallLocations[i] = wallLocations[randomLocation];
            wallLocations[randomLocation] = temp;
        }

        var spaces = TileWidth * TileHeight;
        var wallSpaces = (int)(spaces * WallSpaceRate);
        var wallsTransform = transform.Find("Walls");
        var transformOffset = transform.position - new Vector3(TileWidth / 2f - 0.5f, 0, TileHeight / 2f - 0.5f);
        for (var i = 0; i < wallSpaces; i++)
        {
            Instantiate(WallObject, new Vector3(wallLocations[i].x, 0, wallLocations[i].y) + transformOffset, Quaternion.identity, wallsTransform);
        }
    }
}
