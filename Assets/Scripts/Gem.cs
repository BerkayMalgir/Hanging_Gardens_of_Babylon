using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Collect coins
            int coinsToCollect = PlayerData.doubleItemCollectionActive ? 2 : 1;
            PlayerData.numberOfCoins += coinsToCollect;
            PlayerData.SaveProgress();
            Destroy(gameObject); // Destroy the gem object
        }
    }
}