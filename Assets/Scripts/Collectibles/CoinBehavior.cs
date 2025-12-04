using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("coinPickup");
            
            // Increment the player's money count
            int currentMoney = PlayerPrefs.GetInt("PlayerCoinCount");
            PlayerPrefs.SetInt("PlayerCoinCount", currentMoney + 1);
            PlayerPrefs.Save();
            
            // Destroy the coin object
            Destroy(gameObject);
        }
    }
}
