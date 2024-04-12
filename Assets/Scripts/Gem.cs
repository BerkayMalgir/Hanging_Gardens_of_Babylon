using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
   private void Update()
   {
      transform.Rotate(0,0,50*Time.deltaTime);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag == "Player") ;
      {
         PlayerManager.numberOfCoins += 1;
         Destroy(gameObject);
      }
   }
}