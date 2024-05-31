using UnityEngine;

public static class PlayerData
{
    public static int numberOfCoins = 0;
    public static int doubleItemCollectionCount = 1000; // Default for debugging
    public static int invincibilityCount = 5; // Default for debugging

    public static bool doubleItemCollectionActive = false;
    public static bool invincibilityActive = false;
    public static bool gameOver = false;
    public static bool isGameStarted = false;

    public static void SaveProgress()
    {
        PlayerPrefs.SetInt("Coins", numberOfCoins);
        PlayerPrefs.SetInt("DoubleItemCollectionCount", doubleItemCollectionCount);
        PlayerPrefs.SetInt("InvincibilityCount", invincibilityCount);
        Debug.Log("Progress Saved: Coins=" + numberOfCoins + ", Double Items=" + doubleItemCollectionCount + ", Invincibility=" + invincibilityCount);
    }

    public static void LoadProgress()
    {
        numberOfCoins = PlayerPrefs.GetInt("Coins", 0);
        doubleItemCollectionCount = PlayerPrefs.GetInt("DoubleItemCollectionCount", 1000);
        invincibilityCount = PlayerPrefs.GetInt("InvincibilityCount", 5);
    }
}