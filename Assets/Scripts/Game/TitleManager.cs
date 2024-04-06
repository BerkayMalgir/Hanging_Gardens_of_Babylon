using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float xSpawn = 0;
    public float tileLenght = 18;
    public int numberOfTiles = 3;
    public Transform playerTransform;
    private List<GameObject> activeTiles = new List<GameObject>();
    

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            if(i==0)
                SpawnTileZ(0);
            else
                SpawnTileZ(Random.Range(0,tilePrefabs.Length));
        }
        
    }

    private void Update()
    {

        if (playerTransform.position.z-35 > zSpawn - (numberOfTiles * tileLenght))
        {
            SpawnTileZ(Random.Range(0,tilePrefabs.Length));
            DeleteTile();
        }
    }


    public void SpawnTileZ(int tileIndex)
    {
        
        GameObject go= Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        activeTiles.Add(go);
        zSpawn += tileLenght;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
  

    
}