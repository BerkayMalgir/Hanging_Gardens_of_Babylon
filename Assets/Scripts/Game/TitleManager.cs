using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float xSpawn = 0;
    public float tileLenght = 18;
    public int numberOfTiles = 3;
    

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
   

    public void SpawnTileZ(int tileIndex)
    {
        Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        zSpawn += tileLenght;
    }
  

    
}